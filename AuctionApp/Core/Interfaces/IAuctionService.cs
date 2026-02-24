using AuctionApp.Data.DTO;
using AuctionApp.Data.Models;

namespace AuctionApp.Core.Interfaces
{
    public interface IAuctionService
    {
        Task<List<AuctionListItemDTO>> GetAuctionsAsync(string? status, string? search);
        Task<AuctionListItemDTO?> GetAuctionByIdAsync(int id);
        Task<List<BidListItemDTO>> GetBidHistoryAsync(int auctionId);

        Task<int> CreateAuctionAsync(CreateAuctionDTO dto, int userId);
        Task<bool> UpdateAuctionAsync(CreateAuctionDTO dto, int UserId);

    }
}
