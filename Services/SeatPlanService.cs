using Dapper;
using Microsoft.Data.SqlClient;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using MovieAppBackend.Models;

namespace MovieAppBackend.Services
{
    public class SeatPlanService : ISeatPlanService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SeatPlanService> _logger;
        private readonly SqlConnection _sqlConnection;
        public SeatPlanService(IConfiguration configuration,ILogger<SeatPlanService> logger) 
        {
            _configuration = configuration; 
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
            _logger = logger;
        }
        public async Task<string> GetReservedSeats(GettingReservedSeats gettingReservedSeats) 
        {
            var query = """
                select *
                FROM Books B
                INNER JOIN BookingInfos BI ON B.BookingInfoId = BI.Id
                LEFT JOIN Purchases P ON BI.Id = P.BookingInfoId
                """;

            try
            {
                var bookDictionary = new Dictionary<string, Book>();

                var result =await _sqlConnection.QueryAsync<Book, BookingInfo, Purchase, Book>(query,
                    (book,bookingInfo,purchase) => 
                    {
                        if (!bookDictionary.TryGetValue(book.UserId + "-" + book.MovieItemId + "-" + book.TheaterId + "-" + book.BookingInfoId, out var currentBook))
                        {
                            currentBook = book;
                            bookDictionary.Add(currentBook.UserId + "-" + currentBook.MovieItemId + "-" + currentBook.TheaterId + "-" + currentBook.BookingInfoId, currentBook);
                        }

                        if (bookingInfo != null) 
                        {
                            currentBook.BookingInfo = bookingInfo;
                        }
                        if (purchase != null) 
                        {
                            currentBook.BookingInfo.Purchase = purchase;
                        }

                        return currentBook;

                    },
                    splitOn: "Id,Id");

                //check the reserved seats according to the theater Id, screen Id, Show time and selected movie
                string reservedSeatIds = "";
                foreach (var bookRecord in result) 
                {
                    if (bookRecord.MovieItemId == gettingReservedSeats.MovieId && bookRecord.TheaterId == gettingReservedSeats.TheaterId) 
                    {
                        BookingInfo bookingInfo = bookRecord.BookingInfo;
                        if (bookingInfo.ScreenId == gettingReservedSeats.ScreenId && bookingInfo.ShowTime == gettingReservedSeats.ShowTime) 
                        {
                            reservedSeatIds = reservedSeatIds + bookingInfo.SelectedSeats;
                        }
                    }
                }
                return reservedSeatIds;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"An error occured while retrieving data from the tables(Books,BookingInfos,Purchases).");
                throw ex;
            }
        }
    }
}
