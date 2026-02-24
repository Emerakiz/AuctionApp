using AuctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cascade relations

            // If user is deleted, delete their auctions
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.User)
                .WithMany(u => u.Auctions)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // If auction is deleted, delete its bids
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)       
                .HasForeignKey(b => b.AuctionId)  
                .OnDelete(DeleteBehavior.Cascade);

            // No automatic delete here to avoid multiple cascade paths error in SQL Server
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)   
                .WithMany(u => u.Bids)     
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // WinningBid is set to null if the auction is deleted, to avoid cascade delete issues
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.WinningBid)
                .WithMany()
                .HasForeignKey(a => a.WinningBidId)
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
