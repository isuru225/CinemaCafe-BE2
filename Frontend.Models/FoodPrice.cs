namespace MovieAppBackend.Frontend.Models
{
    public class FoodPrice
    {
        public int FoodId { get; set; }
        public int FoodTypeId { get; set; }
        public string FoodName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
