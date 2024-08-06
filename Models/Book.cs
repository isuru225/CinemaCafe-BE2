using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class Book
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public int MovieItemId { get; set; }
        [Key]
        public int TheaterId { get; set; }
        [Key]
        public int BookingInfoId { get; set; }
        public User User { get; set; }
        public MovieItem MovieItem { get; set; }
        public Theater Theater { get; set; }
        public BookingInfo BookingInfo { get; set; }
    }
}
