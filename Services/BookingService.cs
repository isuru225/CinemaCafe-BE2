using Dapper;
using Microsoft.Data.SqlClient;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using MovieAppBackend.Models;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MovieAppBackend.Services
{
    public class BookingService : IBookingService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _sqlConnection;
        private readonly ILogger<BookingService> _logger;
        public BookingService(IConfiguration configuration, ILogger<BookingService> logger)
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
            _logger = logger;
        }
        public async Task<object> AddBookingInfo(Booking booking)
        {
            using (var connection = _sqlConnection)
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {

                    try
                    {

                        List<string> alreadySelectedSeats = await CheckSeatAvailability(booking?.SeatIds, connection, transaction);
                        if (alreadySelectedSeats.Count > 0)
                        {
                            return new { seatList = alreadySelectedSeats, bookingInfoId = 0 };
                        }
                        else
                        {
                            // Insert into BookingInfo table
                            var bookingInfoId = await connection.QuerySingleAsync<int>(
                                "INSERT INTO BookingInfos (SelectedSeats, ShowTime, AdultTicketQty, ChildTicketQty, ScreenId, Experience) VALUES (@SelectedSeats, @ShowTime, @AdultTicketQty, @ChildTicketQty, @ScreenId, @Experience); SELECT CAST(SCOPE_IDENTITY() as int);",
                                new
                                {
                                    SelectedSeats = booking.SeatIds,
                                    ShowTime = booking.ShowTime,
                                    AdultTicketQty = booking.AdultTicketCount,
                                    ChildTicketQty = booking.ChildTicketCount,
                                    ScreenId = booking.ScreenId,
                                    Experience = booking.Experience
                                },
                                transaction
                            );



                            // Insert into Book table
                            //book.BookingInfoId = bookingInfoId;
                            var affectedRows = await connection.ExecuteAsync(
                                "INSERT INTO Books (UserId, MovieItemId, TheaterId, BookingInfoId) VALUES (@UserId, @MovieItemId, @TheaterId, @BookingInfoId);",
                                new
                                {
                                    UserId = booking.UserId,
                                    MovieItemId = booking.MovieId,
                                    TheaterId = booking.TheaterId,
                                    BookingInfoId = bookingInfoId
                                },
                                transaction
                            );

                            // Commit transaction if both inserts are successful
                            transaction.Commit();
                            List<string> emptyList = new List<string>();
                            return new { seatList = emptyList, bookingInfoId };
                        }

                    }
                    catch
                    {
                        // Rollback transaction if any error occurs
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<List<string>> CheckSeatAvailability(string seatIds, SqlConnection connection, SqlTransaction transaction)
        {
            var query = """
                    SELECT *
                    FROM BookingInfos
                    """;
            List<BookingInfo> bookingInfos = new List<BookingInfo>();

            try
            {

                var result = await connection.QueryAsync<BookingInfo>(query, transaction: transaction);
                bookingInfos = result.ToList();

                List<string> requestedSeats = new List<string>();
                List<string> alreadySelectedSeats = new List<string>();
                requestedSeats = seatIds.Split(',').ToList();

                foreach (var bookingInfo in bookingInfos)
                {
                    foreach (var requestedSeat in requestedSeats)
                    {
                        List<string> bookedSeats = new List<string>();
                        bookedSeats = bookingInfo?.SelectedSeats.Split(",").ToList();
                        foreach (var bookedSeat in bookedSeats)
                        {
                            if (bookedSeat == requestedSeat)
                            {
                                alreadySelectedSeats.Add(requestedSeat);
                            }
                        }
                    }
                }

                return alreadySelectedSeats;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving data from BookingInfos table.");
                throw ex;
            }
        }
    }
}
