using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppBackend.Data;
using MovieAppBackend.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieAppBackend.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public MovieAppDbContext _movieAppDbContext;

        public HomeController(MovieAppDbContext movieAppDbContext)
        {
            _movieAppDbContext = movieAppDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieItem>>> GetMovies() 
        {

            if (_movieAppDbContext.Movie == null)
            {
                return NotFound();
            }
            else
            { 
                //var hello=  await _movieAppDbContext.Movie.Include("Theater").ToListAsync();
                var hello = await _movieAppDbContext.Movie.ToListAsync();
                return hello;
            }
        }

        [HttpPost]
        public async Task<ActionResult<MovieItem>> AddNewMovie(MovieItem movie) 
        {
            _movieAppDbContext.Movie.Add(movie);
            await _movieAppDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovies), new { id = movie.Id , movie});
        }
    }
}
