using AuctionApp.Data.Models;

namespace AuctionApp.Data.Interfaces
{
    public interface IAuctionRepo
    {
        public IQueryable<Auction> QueryAuctions();
        public Task AddAsync(Auction auction);
        public void Update(Auction auction);
        public void Remove(Auction auction);

        Task SaveChanges();

    }
}
