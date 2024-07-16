using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppBackend.Data;
using MovieAppBackend.Models;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public IMovieService _movieService;

        public MovieController(IMovieService movieService) 
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult> GetMovies()
        {
            try
            {
                var result = await _movieService.GetAllMovies();
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("movie_id")]
        public async Task<ActionResult> GetMovieForGivenId([FromQuery] int id ) 
        {
            try
            {
                var result = await _movieService.GetMovieForGivenId(id);
                return Ok(result); 

            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }


    }
}
