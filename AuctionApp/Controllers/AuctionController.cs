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
            try
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
            catch (ArgumentException)
            {
                return BadRequest("status must be: active, closed, or all");
            }
        }

        // GET: api/Auction/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            return Ok(auction);
        }

        // GET: api/Auction/bidHistory?auctionId=5
        [HttpGet("bidHistory")]
        public IActionResult GetBidHistory([FromQuery] int auctionId)
        {
            return Ok($"You requested bid history for auction ID: {auctionId}");
        }

        // POST: api/Auction
        [HttpPost]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var createdAuction = await _auctionService.CreateAuctionAsync(dto, userId);

            return CreatedAtAction(nameof(GetAuctionById), new { id = createdAuction.AuctionId }, null);

        }

        // PUT: api/Auction/5
        [HttpPut("{id}")]
        public IActionResult UpdateAuction(int id, [FromBody] object auctionData)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        }


    }
}
