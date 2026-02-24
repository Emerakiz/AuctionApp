using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        // GET: api/User
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok("This is the UserController");
        }

        // GET: api/User/5
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            return Ok($"Get user with ID: {id}");
        }

        // POST: api/User/register
        [HttpPost("register")]
        public IActionResult CreateUser()
        {
            return Ok("Create a new user");
        }

        // api/User/login
        [HttpPost("login")]
        public IActionResult LoginUser()
        {
            return Ok("Login user");
        }

    }
}
