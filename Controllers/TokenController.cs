using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        public ITokenService _TokenService;
        private readonly ILogger _logger;
        public TokenController(ITokenService TokenService,ILogger<TokenController> logger) 
        { 
            _TokenService = TokenService;
            _logger = logger;
        }
        
        [HttpPost("gettoken")]
        public object GetJWTToken([FromBody] int id)
        {
            try
            {
                var result = _TokenService.GetToken(id);
                _logger.LogInformation("Token has been created successfullly!");
                return result;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occured while creating the token!");
                return new { data = "" };             
            }
        }

    }
}
