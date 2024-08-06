namespace MovieAppBackend.Frontend.Models
{
    public class GettingReservedSeats
    {
        public int TheaterId { get; set; }
        public int ScreenId { get; set; }
        public int MovieId { get; set; }
        public DateTime ShowTime { get; set; }
    }
}
