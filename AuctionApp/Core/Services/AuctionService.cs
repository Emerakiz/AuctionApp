using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AuctionApp.Core.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepo _repo;
        private readonly IMapper _mapper;
        public AuctionService(IAuctionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // Create and post an Auction
        public async Task<int> CreateAuctionAsync(CreateAuctionDTO dto, int userId)
        {

            // Map DTO to auction entity
            var auction = _mapper.Map<Auction>(dto);

            // Set additional properties
            auction.UserId = userId;
            auction.StartDate = DateTime.UtcNow;
            auction.EndDate = DateTime.UtcNow.AddDays(3);


            await _repo.AddAsync(auction);
            await _repo.SaveChanges();

            return auction.AuctionId;

        }

        // Retrieves a single auction by its ID, including details.
        public Task<AuctionListItemDTO?> GetAuctionByIdAsync(int id)
        {
            var query = _repo.QueryAuctions()
                .Where(a => a.AuctionId == id)
                .ProjectTo<AuctionListItemDTO>(_mapper.ConfigurationProvider);

            return query.FirstOrDefaultAsync();
        }

        // Retrieves a list of auctions filtered by status and search criteria.
        public async Task<List<AuctionListItemDTO>> GetAuctionsAsync(string? status, string? search)
        {
            var query = _repo.QueryAuctions();
            var now = DateTime.UtcNow;

            // Status filter
            switch (status?.ToLower())
            {
                case "active":
                    query = query.Where(a => a.StartDate <= now && a.EndDate >= now);
                    break;

                case "closed":
                    query = query.Where(a => a.EndDate < now);
                    break;

                case null:
                case "":
                case "all":
                    break;

                default:
                    throw new ArgumentException("Invalid status value");
            }

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();

                query = query.Where(a =>
                    a.Title.ToLower().Contains(term));
            }

            // Sort filter - default is by end date ascending, closed auctions sort last
            query = query
                .OrderBy(a => a.EndDate < now)
                .ThenBy(a => a.EndDate);

            // AutoMapper projection to DTO
            var dtos = await query
                .ProjectTo<AuctionListItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            // Check if auction is active based on end date
            foreach (var dto in dtos)
            {
                dto.IsActive = dto.EndDate >= now;
            }

            return dtos;
        }

        public Task<List<BidListItemDTO>> GetBidHistoryAsync(int auctionId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing auction with the provided data from the specified user.
        /// </summary>
        /// <param name="dto">The data transfer object containing updated auction information.</param>
        /// <param name="userId">The ID of the user performing the update.</param>
        /// <returns>True if the auction was updated successfully; otherwise, false.</returns>
        public async Task<bool> UpdateAuctionAsync(CreateAuctionDTO dto, int userId, int auctionId)
        {
            var auction = await _repo.QueryAuctions()
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return false;
            }

            // Cannot update if there is any bids on the auction
            if (auction.CurrentPrice > dto.StartingPrice)
            {
                return false; 
            }

            // Uppdate auction properties based on DTO
            auction.Title = dto.Title;
            auction.Description = dto.Description;
            auction.StartingPrice = dto.StartingPrice;


            _repo.Update(auction);
            await _repo.SaveChanges();

            return true;
        }
    }
}
