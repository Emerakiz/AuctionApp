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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves a list of all users in the system.
        /// </summary>
        /// <remarks>This method asynchronously obtains user data from the user service. If no users are
        /// available, the response will indicate 404 Not Found. Callers should handle both success and not found
        /// responses appropriately.</remarks>
        /// <returns>An <see cref="IActionResult"/> that contains a collection of users with a status code 200 (OK) if users are
        /// found; otherwise, a status code 404 (Not Found) if no users exist.</returns>
        // GET: api/User
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();

            if (result == null)
            {
                return NotFound("No users found.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets a user by their unique identifier.
        /// </summary>
        /// <remarks>This endpoint retrieves a user based on the provided ID. If no user is found with the specified ID, a NotFound response is returned.</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/User/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (result == null)
            {
                return NotFound("No user found");
            }

            return Ok(result);
        }

        /// <summary>
        /// Registers a new user with the specified username and password.
        /// </summary>
        /// <remarks>This method handles user registration requests and returns appropriate HTTP responses
        /// based on the outcome. If registration fails due to validation errors or exceptions, a Bad Request response
        /// is returned with an error message.</remarks>
        /// <param name="username">The unique username for the new user. This value must not be null, empty, or already in use.</param>
        /// <param name="password">The password for the new user account. The password must meet the application's security requirements, such
        /// as minimum length and complexity.</param>
        /// <returns>An IActionResult that indicates the result of the registration operation. Returns a 200 OK response if
        /// registration is successful; otherwise, returns a 400 Bad Request response if validation fails or an error
        /// occurs.</returns>
        // POST: api/User/register
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDTO dto)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(dto);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authenticates a user with the provided credentials and returns a token if the login is successful.
        /// </summary>
        /// <remarks>This endpoint requires the caller to be authorized. If authentication fails or an
        /// error occurs, the method returns an unauthorized response with the error message.</remarks>
        /// <param name="dto">An object containing the user's login credentials, including the username and password required for
        /// authentication. Cannot be null.</param>
        /// <returns>A 201 Created response containing a token if authentication succeeds; otherwise, a 401 Unauthorized response
        /// if authentication fails.</returns>
        // api/User/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO dto)
        {
            try
            {
                var token = await _userService.LoginUserAsync(dto.Username, dto.Password);

                return Created("", new TokenResponseDto { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _userService.UpdateUserAsync(userId, dto);

                if (!result)
                {
                    return BadRequest("Failed to update user.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
