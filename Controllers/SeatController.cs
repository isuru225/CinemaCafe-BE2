using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ILogger<SeatController> _logger;
        private ISeatPlanService _seatPlanService;
        public SeatController(ILogger<SeatController> logger, ISeatPlanService seatPlanService) 
        {
            _logger = logger; 
            _seatPlanService = seatPlanService;
        }


        [HttpGet]
        [Authorize]
        public string GetSelectedSeats()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            string seatList = null;

            if (identity != null)
            {

                IEnumerable<Claim> claims = identity.Claims;
                foreach (Claim claim in claims)
                {
                    if (claim.Type == "seatList")
                    {
                        seatList = claim.Value;
                    }
                }
            }

            return seatList;

            //return "xss";
        }
        [HttpGet("reservedseats")]
        public async Task<IActionResult> GetReservedSeatIds([FromQuery] GettingReservedSeats gettingReservedSeats) 
        {
            try 
            {
                var result = await _seatPlanService.GetReservedSeats(gettingReservedSeats);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"");
                return BadRequest(ex.Message);
            }
        }
    }
}
