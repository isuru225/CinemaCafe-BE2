namespace MovieAppBackend.Models
{
    public class FoodBeverage
    {
            public int Id { get; set; }
            public string Name { get; set; }                
            public decimal UnitPrice { get; set; }
            public string Type { get; set; }
            public List<Theater> Theaters { get; set; }
            public List<Purchase> purchases { get; set; }
    }
}
