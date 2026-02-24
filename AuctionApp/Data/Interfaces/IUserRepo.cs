using AuctionApp.Data.Models;

namespace AuctionApp.Data.Interfaces
{
    public interface IUserRepo
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIdAsync(int id);
        public void AddUserAsync(User user);
        Task SaveChanges();
    }
}
