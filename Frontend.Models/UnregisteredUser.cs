using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Frontend.Models
{
    public class UnregisteredUser
    {
        [Key]
        public string Id { get; set; } 
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? MobileNumber { get; set; }
        public bool IsRegistered { get; set; }
    }
}
