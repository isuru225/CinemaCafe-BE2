namespace MovieAppBackend.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string TheaterName { get; set;}
        public string Location { get; set;}
        public List<Screen>? Screens { get; set;}
        public List<FoodBeverage>? FoodBeverages { get; set;}
    }
}
