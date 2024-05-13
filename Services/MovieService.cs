using Dapper;
using MovieAppBackend.IServices;
using Microsoft.Data.SqlClient;
using MovieAppBackend.Data;
using MovieAppBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MovieAppBackend.Services
{
    public class MovieService : IMovieService
    {
        private MovieAppDbContext _movieAppDbContext;
        private IConfiguration _configuration;
        private ILogger<MovieService> _logger;
        private readonly SqlConnection _sqlConnection;

        public MovieService(MovieAppDbContext movieAppDbContext, IConfiguration configaration, ILogger<MovieService> logger)
        {
            _movieAppDbContext = movieAppDbContext;
            _configuration = configaration;
            _logger = logger;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
        }

        //public async Task<ActionResult<IEnumerable<Theater>>> GetTheatersForGivenMovie(int MovieID)
        //{
        //    var Theaters = _movieAppDbContext
        //                   .Movie
        //                   .Where(m => m.Id == MovieID)
        //                   .SelectMany(m => m.MovieIt)
        //    return 0;
        //}

        public async Task<IEnumerable<MovieItem>> GetAllMovies()
        {
            var query = """
                            SELECT *
                            FROM MOVIE
                """;

            List<MovieItem> movies = new List<MovieItem>(); 

            try
            {
                var result = await _sqlConnection.QueryAsync<MovieItem>(query);
                movies = result.ToList();
                return movies;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occured while getting the data from the movie table.");
                return movies;
            }            

        }

        public async Task<MovieItem> GetMovieForGivenId(int movieId)
        {

            var query = """
                            SELECT * FROM 
                            Movie m 
                            INNER JOIN Screening s ON m.Id = s.MovieId 
                            INNER JOIN Screen sc ON sc.Id = s.ScreenId
                            INNER JOIN Theaters t ON t.Id = sc.TheaterId 
                            WHERE m.Id = @Id 
                    """;

            Dictionary<int, MovieItem> movieDictionary = new Dictionary<int, MovieItem>();

            try
            {
                var movies = await _sqlConnection.QueryAsync<MovieItem, Screen, Theater, MovieItem>(
                            query,
                            (movie, screen, theater) =>
                            {
                                if (!movieDictionary.TryGetValue(movie.Id, out MovieItem movieEntry))
                                {
                                    movieEntry = movie;
                                    movieEntry.screens = new List<Screen>();
                                    movieDictionary.Add(movieEntry.Id, movieEntry);
                                }

                                screen.theater = theater;
                                movieEntry.screens.Add(screen);

                                return movieEntry;
                            },
                            new { Id = movieId },
                            splitOn: "Id,Id,Id" // Splitting on "Id" for Movie and Screening
                        );

                return movies.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while quering the data base.");
                return new MovieItem();
            }

        }

    }
}
