using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        IUserService _userService;
        public UserController(IConfiguration configuration,IUserService userService) 
        {
           _configuration = configuration;
           _userService = userService;
        }
        [HttpPost("adduser")]
        public async Task<IActionResult> AddUnregisteredUser([FromBody] int movieId) 
        {
            try 
            {
                var result = await _userService.AddUnregisteredUsers(movieId);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }

        }
    }
}
