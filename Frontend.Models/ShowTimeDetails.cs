namespace MovieAppBackend.Frontend.Models
{
    public class ShowTimeDetails
    {
        public int TheaterId { get; set; }
        public int ScreenId { get; set; }
        public List<DateTime> ShowTime { get; set; }
        public string TheaterName { get; set; }
        public string Location { get; set; }
        public List<string> Experience { get; set; }
    }
}
