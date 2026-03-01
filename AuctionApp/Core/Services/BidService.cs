using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using AuctionApp.Data.Repo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Core.Services
{
    public class BidService : IBidService
    {
        private readonly IAuctionRepo _auctionRepo;
        private readonly IUserRepo _userRepo;
        private readonly IBidRepo _bidRepo;
        private readonly IMapper _mapper;

        public BidService(IAuctionRepo auctionRepo, IBidRepo bidRepo, IMapper mapper, IUserRepo userRepo)
        {
            _auctionRepo = auctionRepo;
            _bidRepo = bidRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        // Deletes a bid if the user is the owner of the bid and the auction is still active and the bid is the latest bid on the auction.
        public async Task<bool> DeleteBidAsync(int bidId, int userId)
        {
            var bid = await _bidRepo.QueryBids()
                .Include(b => b.Auction)
                .FirstOrDefaultAsync(b => b.BidId == bidId);

            if (bid == null)
            {
                throw new Exception("Bid not found.");
            }

            if (bid.UserId != userId)
            {
                throw new Exception("You can only delete your own bids.");
            }


            if (bid.Auction.EndDate < DateTime.UtcNow)
            {
                throw new Exception("Auction is closed.");
            }

            // Get the highest bid on the auction
            var highestBid = await HigestBidOnAuctionAsync(bid.AuctionId);

            // Check if this bid is the highest (latest) bid
            if (highestBid?.BidId != bidId)
            {
                throw new Exception("You can only delete the latest bid on the auction.");
            }

            _bidRepo.RemoveBidAsync(bid);
            await _bidRepo.SaveChangesAsync();

            // Find the new highest bid after deletion
            var newHighestBid = await _bidRepo.QueryBids()
                .Where(b => b.AuctionId == bid.AuctionId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();

            // Update auction current price, fall back to starting price if no bids left
            bid.Auction.CurrentPrice = newHighestBid?.Amount ?? bid.Auction.StartingPrice;

            _auctionRepo.Update(bid.Auction);
            await _auctionRepo.SaveChanges();
            return true;
        }

        // Validates and places a bid on an auction.
        // Ensuring the bid amount is higher than the current highest bid and that the auction is active.
        // Cant place a bid on your own auctions.
        public async Task<BidListItemDTO> PlaceBidAsync(PlaceBidDTO dto, int userId, int auctionId)
        {
            var auction = await _auctionRepo.QueryAuctions()
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            // Checks
            if (auction == null)
            {
                throw new Exception("Auction not found.");
            }
            if (auction.UserId == userId)
            {
                throw new Exception("You cannot place a bid on your own auction.");
            }
            if(auction.EndDate < DateTime.UtcNow)
            {
                throw new Exception("Auction is closed.");
            }

            var current = auction.CurrentPrice ?? auction.StartingPrice;
            if (dto.Amount <= current)
            {
                throw new Exception("Amount too low");
            }


            var bid = new Bid
            {
                Amount = dto.Amount,
                BidTime = DateTime.UtcNow,
                UserId = userId,
                AuctionId = auctionId
            };
          

            auction.CurrentPrice = dto.Amount;
            await _bidRepo.AddBidAsync(bid);
            _auctionRepo.Update(auction);

            await _bidRepo.SaveChangesAsync();
            await _auctionRepo.SaveChanges();



            // Return the newly placed bid as a DTO
            var result = _mapper.Map<BidListItemDTO>(bid);
            var userName = await _userRepo.GetUserByIdAsync(userId);
            result.Name = userName?.Name;

            return result;

        }

        // Retrieves the highest bid for a specific auction.
        public async Task<Bid> HigestBidOnAuctionAsync(int auctionId)
        {
           
            var bid = await _bidRepo.QueryBids()
                    .Where(a => a.AuctionId == auctionId)
                    .OrderByDescending(b => b.Amount)
                    .FirstOrDefaultAsync();

            if (bid == null)
            {
                throw new Exception("No bids have been placed.");
            }

            return bid;
        }
    }
}
