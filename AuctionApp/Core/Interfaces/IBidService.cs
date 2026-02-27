using AuctionApp.Data.DTO;

namespace AuctionApp.Core.Interfaces
{
    public interface IBidService
    {
        Task<BidListItemDTO> PlaceBidAsync(PlaceBidDTO dto, int userId, int auctionId);
        Task<bool> DeleteBidAsync(int bidId, int userId);

    }
}
