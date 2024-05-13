namespace MovieAppBackend.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public decimal VAT { get; set; }
        public List<FoodBeverage> foodBeverages { get; set; }
        public int BookingInfoId { get; set; }
        public BookingInfo BookingInfo { get; set; }
    }
}
