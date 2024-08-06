using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private IBookingService _bookingService;
        public BookingController(ILogger<BookingController> logger,IBookingService bookingService) 
        {
            _logger = logger;
            _bookingService = bookingService;  
        }

        [HttpPost("bookinginfo")]
        public async Task<IActionResult> addBookingInfo([FromBody] Booking booking) 
        {
            try
            {
                var result =await _bookingService.AddBookingInfo(booking);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"");
                return BadRequest(ex);
            }
            
        }
    }
}
