using Microsoft.AspNetCore.Identity;

namespace MovieAppBackend.Models
{
    public class RegisterUsers : IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
