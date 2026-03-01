using AuctionApp.Data.DTO;
using AuctionApp.Data.Models;

namespace AuctionApp.Core.Interfaces
{
    public interface IBidService
    {
        Task<BidListItemDTO> PlaceBidAsync(PlaceBidDTO dto, int userId, int auctionId);
        Task<bool> DeleteBidAsync(int bidId, int userId);

        Task<Bid> HigestBidOnAuctionAsync(int auctionId);

    }
}
