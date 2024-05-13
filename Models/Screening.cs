using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class Screening
    {
        [Key]
        public int MovieId { get; set; }
        [Key]
        public int ScreenId { get; set; }
        [Key]
        public DateTime ShowTime { get; set; }
        public Screen screen { get; set; }
    }
}
