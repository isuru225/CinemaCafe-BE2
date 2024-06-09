using MovieAppBackend.Frontend.Models;
using MovieAppBackend.Models;

namespace MovieAppBackend.IServices
{
    public interface ITheater
    {
        public Task<List<ShowTimeDetails>> GetShowTime(int movieId);
        public Task<List<Theater>> GetTheaters();
        public Task<List<Experience>> GetAllExperiences();
    }
}
