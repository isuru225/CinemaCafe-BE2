using Microsoft.AspNetCore.Mvc;
using MovieAppBackend.Frontend.Models;

namespace MovieAppBackend.IServices
{
    public interface ISeatPlanService
    {
        public Task<string> GetReservedSeats(GettingReservedSeats gettingReservedSeats);
    }
}
