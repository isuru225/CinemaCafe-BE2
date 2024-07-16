using MovieAppBackend.Frontend.Models;
using MovieAppBackend.Models;

namespace MovieAppBackend.IServices
{
    public interface ITheater
    {
        public Task<List<ShowTimeDetails>> GetShowTime(int movieId);
        public Task<List<ShowTimeDetails2>> GetShowTime2(int movieId);
        public Task<List<Theater>> GetTheaters();
        public Task<Theater> GetSelectedTheater(int id);
        public Task<List<Experience>> GetAllExperiences();
        public Task<List<ShowTimeDetails2>> GetShowTimeForAllMovies();
    }
}
