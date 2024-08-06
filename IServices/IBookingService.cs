using Microsoft.Data.SqlClient;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.Models;

namespace MovieAppBackend.IServices
{
    public interface IBookingService
    {
        public Task<Object> AddBookingInfo(Booking booking);
        public Task<List<string>> CheckSeatAvailability(string seatIds, SqlConnection connection, SqlTransaction transaction);
    }
}
