using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private IReservationService _reservationService;
        public ReservationController(ILogger<ReservationController> logger, IReservationService reservationService)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        [HttpGet("getfoodbev")]
        public async Task<IActionResult> GetFoodBeverages([FromQuery] int theaterId) 
        {
            try
            {
                var result = await _reservationService.GetFoodBeverages(theaterId);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
            
        }

        [HttpPost("orderfoods")]
        public async Task<IActionResult> AddOrderedFoods([FromBody] OrderedFoods orderedFoods) 
        {
            try
            {
                var result = await _reservationService.AddOrderedFoods(orderedFoods);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getfoodprice")]
        public async Task<IActionResult> GetOrderedFoodsPrice([FromQuery] int bookingId) 
        {
            try
            {
                var result = await _reservationService.GetOrderedFoodsPrice(bookingId);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }


    }
}
