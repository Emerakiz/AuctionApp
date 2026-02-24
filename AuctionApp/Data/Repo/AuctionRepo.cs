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
            return _context.Auctions.AsNoTracking();
        }

        // Add a new auction
        public async Task AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await SaveChanges();
        }

        // Remove an existing auction
        public void Remove(Auction auction)
        {
            _context.Auctions.Remove(auction);
            _context.SaveChangesAsync();
        }

        // Update an existing auction
        public void Update(Auction auction)
        {
            _context.Auctions.Update(auction);
            _context.SaveChangesAsync();
        }

        // Save changes to the database
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
