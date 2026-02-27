using AuctionApp.Core.Services;
using AuctionApp.Data.Models;

namespace AuctionApp.Data.Interfaces
{
    public interface IBidRepo
    {
        public IQueryable<Bid> QueryBids();
        public Task<Bid?> GetHigestBidByAuctionAsync(int bidId);
        public Task AddBidAsync(Bid bid);
        public void RemoveBidAsync(Bid bid);

        Task SaveChangesAsync();
    }
}
