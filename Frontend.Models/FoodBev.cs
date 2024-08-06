using MovieAppBackend.Models;

namespace MovieAppBackend.Frontend.Models
{
    public class FoodBev
    {
        public int TheaterId { get; set; }
        public List<FoodBeverage> FoodBeverages { get; set; }
    }
}
