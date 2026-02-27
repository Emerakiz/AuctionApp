using AuctionApp.Data.Models;

namespace AuctionApp.Data.Interfaces
{
    public interface IBidRepo
    {
        public Task<List<Bid>> GetBidsByAuctionIdAsync(int auctionId);
        public Task AddBidAsync(Bid bid);
        public Task RemoveBidAsync(Bid bid);

        Task SaveChangesAsync();
    }
}
