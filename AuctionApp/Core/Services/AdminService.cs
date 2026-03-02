using AuctionApp.Core.Interfaces;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepo _userRepo;
        private readonly IAuctionRepo _auctionRepo;
        private readonly IBidRepo _bidRepo;

        public AdminService(IUserRepo userRepo, IAuctionRepo auctionRepo, IBidRepo bidRepo)
        {
            _userRepo = userRepo;
            _auctionRepo = auctionRepo;
            _bidRepo = bidRepo;
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

        public async Task<bool> DisableAuctionAsync(int auctionId, int adminId)
        {
            var auction = await _auctionRepo.QueryAuctions()
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
            {
                throw new ArgumentException("Auction not found");
            }

            auction.IsActive = !auction.IsActive;
            _auctionRepo.Update(auction);
            await _auctionRepo.SaveChanges();
            return true;
        }

        public async Task<bool> DisableUserAsync(int userId, int adminId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }


            user.IsActive = !user.IsActive;
            _userRepo.UpdateUser(user);

            if(!user.IsActive)
            {
                var userBids = await _bidRepo.QueryBids()
               .Include(b => b.Auction)
               .Where(b => b.UserId == userId)
               .ToListAsync();

                var affectedAuctionIds = userBids
                    .Select(b => b.AuctionId)
                    .Distinct()
                    .ToList();

                // Remove all users bids
                foreach (var bid in userBids)
                {
                    _bidRepo.RemoveBidAsync(bid);
                }

                await _bidRepo.SaveChangesAsync();

                // Recalculate current price for each affected auction
                foreach (var auctionId in affectedAuctionIds)
                {
                    var auction = await _auctionRepo.QueryAuctions()
                        .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

                    if (auction == null)
                    {
                        continue;
                    }

                    var newHighestBid = await _bidRepo.QueryBids()
                        .Where(b => b.AuctionId == auctionId)
                        .OrderByDescending(b => b.Amount)
                        .FirstOrDefaultAsync();

                    auction.CurrentPrice = newHighestBid?.Amount ?? auction.StartingPrice;
                    _auctionRepo.Update(auction);
                }
            }

            await _userRepo.SaveChanges();
            return true;
        }

    }
}
