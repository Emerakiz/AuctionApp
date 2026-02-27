using AuctionApp.Data.Models;

namespace AuctionApp.Data.Interfaces
{
    public interface IUserRepo
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIdAsync(int id);
        public Task AddUserAsync(User user);

        void DeleteUser(User user);
        Task SaveChanges();
    }
}
