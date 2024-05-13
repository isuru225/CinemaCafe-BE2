using Microsoft.EntityFrameworkCore;

namespace MovieAppBackend.Data
{
    public class MovieAppDbContext:DbContext
    {
        public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options)
        {
            
        }

        public DbSet<MovieAppBackend.Models.MovieItem> Movie { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.BookingInfo> BookingInfos { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.FoodBeverage> FoodBeverages { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.Theater> Theaters { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.User> Users { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.Purchase> Purchases { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.Book> Books { get; set; } = default!;
        public DbSet<MovieAppBackend.Models.Screening> screenings { get; set; } = default!;
    }
}


//what is a DbContext?
//