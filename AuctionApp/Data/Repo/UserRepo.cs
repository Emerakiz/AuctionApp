using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuctionApp.Data.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(User user)
        {   
            _context.Users.Remove(user);

        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();

        }

        public Task<User?> GetByUserNameAsync(string username)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
