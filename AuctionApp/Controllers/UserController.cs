using AuctionApp.Core.Interfaces;
using AuctionApp.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();

            if (result == null)
            {
                return NotFound("No users found.");
            }

            return Ok(result);
        }

        // GET: api/User/5
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var result = _userService.GetUserByIdAsync(id);

            if (result == null)
            {
                return NotFound("No user found");
            }

            return Ok(result);
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(string username, string password)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(username, password);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserDTO dto)
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
        
    }
}
