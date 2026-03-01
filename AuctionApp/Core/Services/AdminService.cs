using AuctionApp.Core.Interfaces;
using AuctionApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepo _userRepo;
        private readonly IAuctionRepo _auctionRepo;

        public AdminService(IUserRepo userRepo, IAuctionRepo auctionRepo)
        {
            _userRepo = userRepo;
            _auctionRepo = auctionRepo;
        }

        public async Task<bool> DeleteAuctionAsync(int auctionId, int userId)
        {
            var auction = await _auctionRepo.QueryAuctions()
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                throw new ArgumentException("Auction not found");
            }

            if (auction.UserId != userId)
            {
                throw new UnauthorizedAccessException("User does not have permission to delete this auction");
            }


            _auctionRepo.Remove(auction);
            await _auctionRepo.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteUserAsync(int userId, int adminUserId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            _userRepo.DeleteUser(user);
            await _userRepo.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            if (!user.IsAdmin)
            {
                throw new UnauthorizedAccessException("User does not have admin privileges");
            }

            return await Task.FromResult(true);
        }

       
    }
}
