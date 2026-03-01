using AuctionApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AuctionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Deletes the auction with the specified identifier if the current user has administrative privileges.
        /// </summary>
        /// <remarks>This operation is restricted to users with administrative rights. If the user is not
        /// authorized, the request is denied. Ensure that the provided auction ID is valid and that the user has the
        /// necessary permissions before calling this method.</remarks>
        /// <param name="id">The unique identifier of the auction to delete. Must correspond to an existing auction.</param>
        /// <returns>An IActionResult that indicates the result of the operation. Returns NoContent if the auction is
        /// successfully deleted; returns Forbid if the user lacks administrative privileges; otherwise, returns
        /// BadRequest with an error message.</returns>
        // DELETE: api/Admin/auction/5
        [Authorize]
        [HttpDelete("auction/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                if (!await _adminService.IsUserAdminAsync(userId))
                {
                    return Forbid("You do not have permission to perform this action.");
                }

                await _adminService.DeleteAuctionAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        /// <summary>
        /// Deletes the user identified by the specified user ID.
        /// </summary>
        /// <remarks>The caller must have administrative privileges to perform this action. If the caller
        /// is not an admin, a forbidden response is returned. Exceptions encountered during the operation result in a
        /// BadRequest response.</remarks>
        /// <param name="id">The unique identifier of the user to delete. Must correspond to an existing user.</param>
        /// <returns>An IActionResult that indicates the result of the delete operation. Returns NoContent if the user is
        /// successfully deleted; otherwise, returns a BadRequest with an error message.</returns>
        // DELETE: api/Admin/user/5
        [Authorize]
        [HttpDelete("user/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (!await _adminService.IsUserAdminAsync(userId))
                {
                    return Forbid("You do not have permission to perform this action.");
                }

                await _adminService.DeleteUserAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("auction/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DisableAuction(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (!await _adminService.IsUserAdminAsync(userId))
                {
                    return Forbid("You do not have permission to perform this action.");
                }

                await _adminService.DisableAuctionAsync(id, userId);
                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("user/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DisableUser(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (!await _adminService.IsUserAdminAsync(userId))
                {
                    return Forbid("You do not have permission to perform this action.");
                }

                await _adminService.DisableUserAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
