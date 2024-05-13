using MovieAppBackend.Models;

namespace MovieAppBackend.IServices
{
    public interface IMovieService
    {
        public Task<IEnumerable<MovieItem>> GetAllMovies();
        public Task<MovieItem> GetMovieForGivenId(int movieId);
    }
}
