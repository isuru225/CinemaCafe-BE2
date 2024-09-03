using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieAppBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MovieAppBackend.Data
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) 
        {
        
        }

    }
}
