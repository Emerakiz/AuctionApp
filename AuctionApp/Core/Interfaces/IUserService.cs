using AuctionApp.Data.DTO;

namespace AuctionApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDTO dto);
        Task<string> LoginUserAsync(string username, string password);
        Task<List<UserDTO>> GetUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, RegisterUserDTO dto);


    }
}
