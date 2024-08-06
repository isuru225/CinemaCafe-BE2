namespace MovieAppBackend.Frontend.Models
{
    public class Booking
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public string SeatIds { get; set; }
        public string ShowTime { get; set; }
        public int AdultTicketCount { get; set; }
        public int ChildTicketCount { get; set; }
        public int ScreenId { get; set; }
        public string Experience { get; set; }
    }
}

