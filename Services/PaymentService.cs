using Dapper;
using Microsoft.Data.SqlClient;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using MovieAppBackend.Models;

namespace MovieAppBackend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReservationService> _logger;
        private readonly SqlConnection _sqlConnection;
        public PaymentService(IConfiguration configuration, ILogger<ReservationService> logger) 
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
            _logger = logger;
        }

        public async Task<int> AddPurchaseInfos(PurchaseInfos purchaseInfos)
        {

            var query = """
                    INSERT INTO Purchases (PaymentMethod,VAT,BookingInfoId) VALUES (@PaymentMethod, @VAT, @BookingInfoId); SELECT CAST(SCOPE_IDENTITY() as int);
                """;

            try
            {
                // Insert into Purchases table
                var purchaseId = await _sqlConnection.QuerySingleAsync<int>(query,
                    new 
                    {
                        PaymentMethod = purchaseInfos.PaymentMenthod,
                        VAT = purchaseInfos.VAT,
                        BookingInfoId = purchaseInfos.BookingInfoId
                    });

                return purchaseId;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"An error occured while inserting values into the purchaseInfo table");
                throw ex;
            }
        }
    }
}
