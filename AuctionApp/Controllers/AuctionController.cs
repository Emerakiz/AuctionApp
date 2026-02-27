using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        /// <summary>
        /// Retrieves a list of auctions filtered by status and search criteria.
        /// </summary>
        /// <remarks>The status parameter is case-insensitive and must be one of the allowed values. If an
        /// invalid status is provided, the method returns a bad request response.</remarks>
        /// <param name="status">The status filter for auctions. Valid values are "active", "closed", or "all". If null or empty, all
        /// auctions are returned.</param>
        /// <param name="search">An optional search term used to filter auctions by title or description. If null or empty, no search
        /// filtering is applied.</param>
        /// <returns>An <see cref="IActionResult"/> containing the filtered list of auctions. Returns a bad request result if the
        /// status parameter is invalid.</returns>
        // GET: api/Auction/?status=active&search=painting
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuctions(
            [FromQuery] string? status,
            [FromQuery] string? search)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                var s = status.Trim().ToLower();
                if (s != "active" && s != "closed" && s != "all")
                {
                    return BadRequest("status must be: active, closed, or all");
                }
            }

            var result = await _auctionService.GetAuctionsAsync(status, search);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the auction with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the auction to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the auction data if found; otherwise, a NotFound result.</returns>
        // GET: api/Auction/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            return Ok(auction);
        }

        /// <summary>
        /// Retrieves the bid history for the specified auction.
        /// </summary>
        /// <param name="auctionId">The unique identifier of the auction for which to retrieve bid history.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of bid records for the specified auction. Returns an
        /// empty collection if no bids have been placed.</returns>
        // GET: api/Auction/bidHistory?auctionId=5
        [HttpGet("bidHistory")]
        public async Task<IActionResult> GetBidHistory([FromQuery] int auctionId)
        {
            var bidHistory = await _auctionService.GetBidHistoryAsync(auctionId);
            return Ok(bidHistory);

        }

        /// <summary>
        /// Creates a new auction using the specified auction details.
        /// </summary>
        /// <param name="dto">An object containing the details of the auction to create. Must not be null.</param>
        /// <returns>A response with status code 201 (Created) and a location header pointing to the newly created auction
        /// resource.</returns>
        // POST: api/Auction
        [HttpPost]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var createdAuction = await _auctionService.CreateAuctionAsync(dto, userId);

            return CreatedAtAction(nameof(GetAuctionById), new { id = createdAuction }, null);

        }


        /// <summary>
        /// Updates an existing auction with the specified identifier using the provided auction details.
        /// </summary>
        /// <param name="id">The unique identifier of the auction to update.</param>
        /// <param name="dto">An object containing the updated auction details. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation. Returns <see
        /// cref="OkObjectResult"/> with the update result if successful; otherwise, <see cref="NotFoundResult"/> if the
        /// auction does not exist.</returns>
        // PUT: api/Auction/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] CreateAuctionDTO dto, int auctionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _auctionService.UpdateAuctionAsync(dto, userId, auctionId);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);

        }


    }
}
