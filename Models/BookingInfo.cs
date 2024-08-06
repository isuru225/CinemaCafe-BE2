namespace MovieAppBackend.Models
{
    public class BookingInfo
    {
        public int Id { get; set; }
        public string SelectedSeats { get; set; }
        public DateTime ShowTime { get; set; }
        public int AdultTicketQty { get; set; }
        public int ChildTicketQty { get; set; }
        public int ScreenId { get; set; }
        public string Experience { get; set; }
        public Purchase Purchase { get; set; }
    }
}
