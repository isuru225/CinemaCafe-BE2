using MovieAppBackend.Frontend.Models;

namespace MovieAppBackend.IServices
{
    public interface IReservationService
    {
        public Task<FoodBev> GetFoodBeverages(int theaterId);
        public Task<object> AddOrderedFoods(OrderedFoods orderedFoods);
        public Task<List<FoodPrice>> GetOrderedFoodsPrice(int bookingId);
    }
}
