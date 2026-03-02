using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data.Repo
{
    public class BidRepo : IBidRepo
    {
        private readonly AppDbContext _context;
        public BidRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddBidAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
            _context.SaveChanges();
        }

        public async Task<Bid?> GetHigestBidByAuctionAsync(int bidId)
        {
            var bid = await _context.Bids
                .Where(b => b.BidId == bidId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();

            return bid;
        }

        public IQueryable<Bid> QueryBids()
        {
            return _context.Bids
                .Include(b => b.User);
        }

        public void RemoveBidAsync(Bid bid)
        {
            _context.Bids.Remove(bid);
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
