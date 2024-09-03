using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.Custom.Exceptions;
using MovieAppBackend.Enum;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        IUserService _userService;
        public UserController(IConfiguration configuration, IUserService userService)
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
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister([FromBody] RegisterInfo registerInfo)
        {
            try
            {
                //This "ModelState" check that the incoming data comply with the data annotation rules define in the
                //RegisterInfo Model
                if (ModelState.IsValid)
                {
                    var result = await _userService.AddRegisteredUser(registerInfo);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Input data is not compatible.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginUser loginUser)
        {
            try
            {
                //This "ModelState" check that the incoming data comply with the data annotation rules define in the
                //RegisterInfo Model
                if (ModelState.IsValid)
                {
                    var result = await _userService.Login(loginUser);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Input data is not compatible.");
                }

            }
            catch (UserNotFoundException ex)
            {
                var errorResponse = new
                {
                    ErrorCode = ErrorCodes.EmailNotFound,
                    Message = "The user with the provided email was not found.",
                    DetailedMessage = ex.Message
                };

                return BadRequest(errorResponse);
            }
            catch (InvalidCredentialException ex)
            {
                var errorResponse = new
                {
                    ErrorCode = ErrorCodes.IncorrectPassword,
                    Message = "Incorrect password",
                    DetailedMessage = ex.Message
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("verify")]
        public async Task<IActionResult> verifyUserByRoles()
        {
            try
            {
                return Ok(new { message = "Registered user.", isRegistered = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles ="Admin,User")]
        [HttpGet("getprofileinfo")]
        public async Task<IActionResult> GetProfileInfo(string email)
        {
            try
            {
                var result = await _userService.GetProfileInfo(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
