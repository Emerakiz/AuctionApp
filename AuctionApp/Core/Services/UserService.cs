using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using AuctionApp.Data.Interfaces;
using AutoMapper;

namespace AuctionApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if(user == null)
            {
                return null;
            }

            var users = _mapper.Map<UserDTO>(user);

            return users;

        }

        public Task<List<UserDTO?>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> LoginUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
