using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class Book
    {
        [Key]
        public int UserId { get; set; }
        public int MovieItemId { get; set; }
        public int TheaterId { get; set; }
        public int BookingInfoId { get; set; }
        public User User { get; set; }
        public MovieItem MovieItem { get; set; }
        public Theater Theater { get; set; }
        public BookingInfo BookingInfo { get; set; }
    }
}
