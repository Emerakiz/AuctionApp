using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;

namespace AuctionApp.Data.Repo
{
    public class BidRepo : IBidRepo
    {
        public Task AddBidAsync(Bid bid)
        {
            throw new NotImplementedException();
        }

        public Task<List<Bid>> GetBidsByAuctionIdAsync(int auctionId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBidAsync(Bid bid)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
