using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class FoodBeverage
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public int Type { get; set; }
        public string FoodName { get; set; }
        public string TypeName { get; set; }
        public decimal UnitPrice { get; set; }
        public List<Theater> Theaters { get; set; }
        public List<Purchase> purchases { get; set; }
    }
}
