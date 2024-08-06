using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class OrderedFoodsBev
    {
        [Key]
        public int Id { get; set; }
        public int FoodId { get; set; }
        public int FoodType { get; set; }
        public int BookingId { get; set; }
        public int Qty { get; set; }
    }
}
