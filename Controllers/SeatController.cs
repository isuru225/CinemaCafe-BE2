using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public string GetSelectedSeats () 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            string seatList = null;

            if (identity != null)
            {
                
                IEnumerable<Claim> claims = identity.Claims;
                foreach (Claim claim in claims)
                {
                    if (claim.Type=="seatList") 
                    {
                        seatList = claim.Value;
                    }
                }
            }

            return seatList;

            //return "xss";
        }
    }
}
