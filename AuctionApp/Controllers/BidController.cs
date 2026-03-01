using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Places a bid on a specified auction using the provided bid details.
        /// </summary>
        /// <remarks>The user must be authenticated to place a bid, as the method retrieves the user ID
        /// from the current user's claims. The method may return an error if the bid fails validation or if other
        /// issues occur during processing.</remarks>
        /// <param name="dto">An object containing the details of the bid to be placed, such as the bid amount and any additional
        /// information required to process the bid.</param>
        /// <param name="auctionId">The unique identifier of the auction on which the bid is being placed.</param>
        /// <returns>An IActionResult that represents the result of the bid placement operation. Returns a success response with
        /// bid details if the bid is placed successfully; otherwise, returns a bad request with an error message.</returns>
        // POST: api/Bid/auction/5
        [Authorize]
        [HttpPost("auction/{auctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes the bid with the specified identifier for the currently authenticated user.
        /// </summary>
        /// <remarks>This action requires the user to be authenticated. If an error occurs during
        /// deletion, a 400 Bad Request response is returned with the error message.</remarks>
        /// <param name="bidId">The unique identifier of the bid to delete. Must be a positive integer.</param>
        /// <returns>An IActionResult that indicates the result of the operation. Returns 200 OK if the bid is successfully
        /// deleted; returns 404 Not Found if the bid does not exist.</returns>
        // DELETE: api/Bid/5
        [Authorize]
        [HttpDelete("{bidId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
