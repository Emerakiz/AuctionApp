using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data.Repo
{
    public class AuctionRepo : IAuctionRepo
    {
        private readonly AppDbContext _context;
        public AuctionRepo(AppDbContext context)
        {
            _context = context;
        }

        // Base query for auctions (read-only) to search, filter, and sort auctions
        public IQueryable<Auction> QueryAuctions()
        {
            return _context.Auctions;
        }

        // Add a new auction
        public async Task AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
        }

        // Remove an existing auction
        public void Remove(Auction auction)
        {
            _context.Auctions.Remove(auction);
        }

        // Update an existing auction
        public void Update(Auction auction)
        {
            _context.Auctions.Update(auction);
        }

        // Save changes to the database
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        // Wipe all auctions from the database (for testing purposes)
        public async Task ClearAllAsync()
        {
            _context.Auctions.RemoveRange(_context.Auctions);
            await _context.SaveChangesAsync();
        }
    }
}
