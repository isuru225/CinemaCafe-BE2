using Dapper;
using Microsoft.Data.SqlClient;
using MovieAppBackend.Custom.Exceptions;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using MovieAppBackend.Models;

namespace MovieAppBackend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReservationService> _logger;
        private readonly SqlConnection _sqlConnection;
        public ReservationService(IConfiguration configuration, ILogger<ReservationService> logger)
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
            _logger = logger;
        }
        public async Task<FoodBev> GetFoodBeverages(int theaterId)
        {
            var query = """
                    SELECT *
                    FROM AvailableFoods AF
                    INNER JOIN FoodBeverages FB ON AF.FoodId = FB.Id AND AF.FoodType = FB.Type
                    WHERE AF.TheaterId = @ID
                    """;
            try
            {
                List<string> foodBeveragesIds = new List<string>();
                List<FoodBeverage> selectedFoodBev = new List<FoodBeverage>();
                var result = await _sqlConnection.QueryAsync<AvailableFoods, FoodBeverage, FoodBeverage>(query,
                (availableFoods, foodBeverage) =>
                {

                    if (!foodBeveragesIds.Contains(foodBeverage.Id + "-" + foodBeverage.Type))
                    {
                        foodBeveragesIds.Add(foodBeverage.Id + "-" + foodBeverage.Type);
                        return foodBeverage;
                    }
                    else
                    {
                        throw new DuplicateValuesException("primary key of foodBeverage table is duplicated");
                    }

                },
                new { @Id = theaterId },
                splitOn: "Id");

                var foodBev = new FoodBev();
                foodBev.TheaterId = theaterId;
                foodBev.FoodBeverages = new List<FoodBeverage>();
                foodBev.FoodBeverages = result.ToList();

                return foodBev;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FoodBeverage>> GetAllFoodsBevs()
        {
            var query = """
                      SELECT *
                      FROM FoodBeverages
                """;
            try
            {
                var result = await _sqlConnection.QueryAsync<FoodBeverage>(query);
                return result?.ToList();
            }
            catch (Exception ex) 
            {
                _logger.LogError("An error occured while retrieving data from FoodBeverages table");
                throw ex;
            }
        }

        public async Task<object> AddOrderedFoods(OrderedFoods orderedFoods)
        {
            var query = """
                    INSERT INTO OrderedFoodBev (FoodId,FoodType,BookingId,Qty) VALUES (@FoodId,@FoodType,@BookingId,@Qty);SELECT CAST(SCOPE_IDENTITY() as int);
                """;

            try
            {
                var result = await _sqlConnection.QuerySingleAsync<int>(
                        query,
                new OrderedFoodsBev
                {
                    FoodId = orderedFoods.FoodId,
                    FoodType = orderedFoods.FoodType,
                    BookingId = orderedFoods.BookingId,
                    Qty = orderedFoods.FoodQty
                }
                );

                List<Object> orderedFoodItems = new List<Object>();
                orderedFoodItems.Add(new { orderedFoods.FoodId, orderedFoods.FoodType });
                return orderedFoodItems;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding food details into OrderedFoodBev table");
                throw ex;
            }


        }

        public async Task<List<FoodPrice>> GetOrderedFoodsPrice(int bookingId)
        {
            var query = """
                    SELECT *
                    FROM OrderedFoodBev
                    WHERE BookingId = @Id
                """;

            try
            {
                var result = await _sqlConnection.QueryAsync<OrderedFoodsBev>(
                    query
                    , new { Id = bookingId });
                // calculate the each food price according the quantity

                try
                {
                    var availableFoodsBevs = await GetAllFoodsBevs();

                    List<FoodPrice> foodPrices = new List<FoodPrice>();

                    foreach (var orderedFoodItem in result) 
                    {
                        for (int x = 0; x < availableFoodsBevs.Count(); x++) 
                        {
                            if ((orderedFoodItem.FoodId == availableFoodsBevs[x]?.Id) && (orderedFoodItem.FoodType == availableFoodsBevs[x]?.Type)) 
                            {
                                FoodPrice foodPrice = new FoodPrice();
                                foodPrice.FoodId = orderedFoodItem.FoodId;
                                foodPrice.FoodTypeId = orderedFoodItem.FoodType;
                                foodPrice.Qty = orderedFoodItem.Qty;
                                foodPrice.Price = orderedFoodItem.Qty * availableFoodsBevs[x].UnitPrice;
                                foodPrice.FoodName = availableFoodsBevs[x].FoodName;

                                foodPrices.Add(foodPrice);

                                break;
                            }
                        }  
                    }

                    return foodPrices;

                }
                catch (Exception ex) 
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while retrieving data for OrderedFoodBev table");
                throw ex;
            }
        
        }
    }
}
