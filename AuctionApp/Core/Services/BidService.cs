using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Core.Services
{
    public class BidService : IBidService
    {
        private readonly IAuctionRepo _auctionRepo;
        private readonly IBidRepo _bidRepo;
        private readonly IMapper _mapper;

        public BidService(IAuctionRepo auctionRepo, IBidRepo bidRepo, IMapper mapper)
        {
            _auctionRepo = auctionRepo;
            _bidRepo = bidRepo;
            _mapper = mapper;
        }

        // Deletes a bid if the user is the owner of the bid and the auction is still active and the bid is the latest bid on the auction.
        public Task<bool> DeleteBidAsync(int bidId, int userId)
        {
            throw new NotImplementedException();
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
                BidTime = DateTime.Now,
                UserId = userId,
                AuctionId = auctionId
            };

            await _bidRepo.AddBidAsync(bid);
            auction.CurrentPrice = dto.Amount;

            await _bidRepo.SaveChangesAsync();

            // Return the newly placed bid as a DTO
            var result = _mapper.Map<BidListItemDTO>(bid);
            return result;

        }
    }
}
