using AuctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data
{
    public class AppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("DefaultConnection");
        }

    }
}
