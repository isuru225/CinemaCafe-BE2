using MovieAppBackend.Models;

namespace MovieAppBackend.Frontend.Models
{
    public class ShowTimes
    {
        public int MovieId { get; set; }
        public List<Screening> screenings { get; set; }
    }
}
