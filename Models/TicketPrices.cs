namespace MovieAppBackend.Models
{
    public class TicketPrices
    {
        public int MovieId { get; set; }
        public int ScreenId { get; set; }
        public decimal adultTicketPrice { get; set; }
        public decimal childTicketPrice { get; set; }
    }
}
