using MovieAppBackend.Models;

namespace MovieAppBackend.Frontend.Models
{
    public class PurchaseInfos
    {
        public string PaymentMenthod { get; set; } 
        public decimal VAT { get; set; }
        public int BookingInfoId { get; set; }
    }
}
