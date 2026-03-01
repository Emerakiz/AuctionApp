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
        private readonly IBidRepo _bidRepo;
        private readonly IMapper _mapper;
        private readonly IBidService _bidService;
        public AuctionService(IAuctionRepo repo, IMapper mapper, IBidRepo bidRepo, IBidService bidService)
        {
            _repo = repo;
            _mapper = mapper;
            _bidRepo = bidRepo;
            _bidService = bidService;
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
        public async Task<AuctionListItemDTO?> GetAuctionByIdAsync(int id)
        {
            var query = await _repo.QueryAuctions()
                .Where(a => a.AuctionId == id)
                .ProjectTo<AuctionListItemDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return query;
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
                    query = query.Where(a => a.EndDate >= now);
                    break;
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

        // Retrieves the bid history for a specific auction, ordered by time placed.
        public async Task<List<BidListItemDTO>> GetBidHistoryAsync(int auctionId)
        {
            var bidHistory = await _bidRepo.QueryBids()
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.BidTime)
                .ProjectTo<BidListItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return bidHistory;
        }

        // Updates an existing auction's details. If there are bids placed on it, you can't change the price.
        public async Task<bool> UpdateAuctionAsync(int auctionId, CreateAuctionDTO dto, int userId)
        {
            var auction = await _repo.QueryAuctions()
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                return false;
            }

            // Uppdate auction properties based on DTO
            auction.Title = dto.Title;
            auction.Description = dto.Description;

            // If bids are placed, you can't change the price. If no bids, update the price to the new starting price.
            if (auction.CurrentPrice <= dto.StartingPrice || auction.CurrentPrice == null)
            {
                auction.StartingPrice = dto.StartingPrice;
            }


            _repo.Update(auction);
            await _repo.SaveChanges();

            return true;
        }

        // Deletes auction
        public async Task<bool> DeleteAuctionAsync(int auctionId, int userId)
        {
            var auction = await _repo.QueryAuctions()
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
            if (auction == null || auction.UserId != userId)
            {
                return false;
            }

            // Check if there are any bids placed on the auction
            var hasBids = await _bidRepo.QueryBids()
                .AnyAsync(b => b.AuctionId == auctionId);

            // Can't delete if there are bids
            if (hasBids)
            {
                return false; 
            }

            _repo.Remove(auction);
            await _repo.SaveChanges();
            return true;
        }
    }
}
