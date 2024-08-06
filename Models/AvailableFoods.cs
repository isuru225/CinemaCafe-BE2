using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class AvailableFoods
    {
        [Key]
        public int TheaterId { get; set; }
        [Key]
        public int FoodId { get; set;}
        [Key]
        public int FoodType { get; set;}
    }
}
