using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        // POST: api/Bid/auction/5
        [HttpPost("auction/{auctionId:int}")]
        public async Task<IActionResult> PlaceBid(
            [FromBody] PlaceBidDTO dto, 
            [FromRoute] int auctionId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _bidService.PlaceBidAsync(dto, userId, auctionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Bid/5
        [HttpDelete("{bidId:int}")]
        public async Task<IActionResult> DeleteBid(int bidId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _bidService.DeleteBidAsync(bidId, userId);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
