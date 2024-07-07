
namespace MovieAppBackend.Frontend.Models
{
    public class ShowTimeDetails
    {
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public int ScreenId { get; set; }
        public List<DateTime> ShowTime { get; set; }
        public string TheaterName { get; set; }
        public string Location { get; set; }
        public List<string> Experience { get; set; }
    }

    public class ShowTimeDetails2
    {
        public int MovieId { get; set; }
        public List<TheaterInfo> theaterInfo { get; set; }
    }

    public class TheaterInfo
    {
        public int Id { get; set; }
        public string TheaterName { get; set; }
        public string Location { get; set; }
        public List<ScreenInfo> screensInfo { get; set; }
    }

    public class ScreenInfo
    {
        public int Id { get; set; }
        public int TheaterId { get; set; }
        public string Experience { get; set; }
        public List<DateTime> showTimes { get; set; }
    }
}

