using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using AuctionApp.Data.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

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
        public Task<int> CreateAuctionAsync(CreateAuctionDTO dto)
        {
            throw new NotImplementedException();
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

        public Task<bool> UpdateAuctionAsync(CreateAuctionDTO dto, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
