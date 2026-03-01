using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserService(IUserRepo userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
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

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();

            return _mapper.Map<List<UserDTO>>(users);
 
        }

        public async Task<string> LoginUserAsync(string username, string password)
        {
            var user = await _userRepo.GetByUserNameAsync(username);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.Password != password)
            {
                throw new Exception("Invalid password");    
            }

            return GenerateToken(user);
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDTO dto)
        {
            var existingUser = await _userRepo.GetByUserNameAsync(dto.Name);
            if (existingUser != null)
            {
                return false;
            }

            var user =_mapper.Map<User>(dto);

            await _userRepo.AddUserAsync(user);
            await _userRepo.SaveChanges();
            return true;
        }

        // JWT token generator
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            var secretKey = _config["JwtSettings:Key"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JwtSettings:Key is missing");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

            
    }
}
