using MovieAppBackend.Models;

namespace MovieAppBackend.Frontend.Models
{
    public class ScreenModified
    {
        public int Id { get; set; }
        public int TheaterId { get; set; }
        public string Experience { get; set; }
        public List<DateTime> showTimes { get; set; }
    }
}
