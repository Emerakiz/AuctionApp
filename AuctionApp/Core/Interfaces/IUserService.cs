using AuctionApp.Data.DTO;

namespace AuctionApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(string username, string password);
        Task<string> LoginUserAsync(string username, string password);
        Task<List<UserDTO?>> GetUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);

    }
}
