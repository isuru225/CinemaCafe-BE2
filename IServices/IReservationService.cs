using MovieAppBackend.Frontend.Models;
using MovieAppBackend.Models;

namespace MovieAppBackend.IServices
{
    public interface IReservationService
    {
        public Task<FoodBev> GetFoodBeverages(int theaterId);
        public Task<object> AddOrderedFoods(OrderedFoods orderedFoods);
        public Task<List<FoodPrice>> GetOrderedFoodsPrice(int bookingId);
        public Task<string> AddExistingUserInfo(User user);
        public Task<Object> GetTicketPriceInfo(GettingTicketInfos gettingTicketInfos);
    }
}
