using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly ILogger<TheaterController> _logger;
        private readonly ITheater _theater;
        public TheaterController(ILogger<TheaterController> logger, ITheater theater)
        {
            _logger = logger;
            _theater = theater;
        }
        [HttpGet("show-time")]
        public async Task<ActionResult> GetShowTime([FromQuery] int movieId)
        {
            try
            {
                var result = await _theater.GetShowTime2(movieId);
                return Ok(result);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting the movie show times");
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("show-time-all")]
        public async Task<ActionResult> GetShowTimeForAllMovies()
        {
            try
            {
                var result = await _theater.GetShowTimeForAllMovies();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting the movie show times");
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-theaters")]
        public async Task<ActionResult> GetAllTheaters()
        {
            try
            {
                var result = await _theater.GetTheaters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting the available theaters.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("theater")]
        public async Task<ActionResult> GetSelectedTheater([FromQuery] int id)
        {
            try
            {
                var result = await _theater.GetSelectedTheater(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting the selected theater.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-experience")]
        public async Task<ActionResult> GetAllTheaterExperiences() 
        {
            try
            {
                var result = await _theater.GetAllExperiences();
                return Ok(result);
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex,"An error occured while running the theater service");
                return BadRequest(ex.Message);
            }
        }


    }
}
