using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieAppBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MovieAppBackend.Data
{
    public class MovieAppDbContext : DbContext
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
        public DbSet<MovieAppBackend.Models.TicketPrices> TicketPrices { get; set; } = default!;
    }



}




//public class ApplicationDbContext : IdentityDbContext<IdentityUser>
//{
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//        : base(options)
//    {
//    }
//}


//what is a DbContext?
//



//---what is IdentityDbContext?---//

//It is an extended version of DbContext. Offering some addidtion features and functionalities to manage the users
//Simply the authentication and authorization processces.
//Entities Managed by IdentityDbContext:
/////////////////////////

//IdentityUser: Represents a user in the identity system, containing properties like UserName, Email, PasswordHash, etc.
//IdentityRole: Represents a role that a user can belong to, like "Admin" or "User".
//IdentityUserRole: Maps users to roles.
//IdentityUserClaim: Represents a claim that a user possesses, like "CanEditProfile".
//IdentityUserLogin: Stores information about external logins (e.g., Google, Facebook).
//IdentityRoleClaim: Represents claims associated with a role.
//IdentityUserToken: Stores tokens associated with a user

//when IdentityDbContext is being used, it will automatically create some tables in database that are helpful for user
//management.


//AspNetUsers
//AspNetRoles
//AspNetUserRoles
//AspNetUserClaims
//AspNetRoleClaims
//AspNetUserLogins
//AspNetUserTokens



//public class MovieAppDbContext : DbContext
//{
//    public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options)
//    {

//    }

//    public DbSet<MovieAppBackend.Models.MovieItem> Movie { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.BookingInfo> BookingInfos { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.FoodBeverage> FoodBeverages { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.Theater> Theaters { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.User> Users { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.Purchase> Purchases { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.Book> Books { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.Screening> screenings { get; set; } = default!;
//    public DbSet<MovieAppBackend.Models.TicketPrices> TicketPrices { get; set; } = default!;
//}

//Waht is ASP.NET core Identity?
//It is a user/membership management system.

//Important
//Swagger does not support JWT authentication and authorization processes.
//But it can be manually configured.