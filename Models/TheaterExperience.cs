using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class TheaterExperience
    {
        [Key]
        public int TheaterId { get; set; }
        [Key]
        public string Experience { get; set; }
    }
}
