using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Frontend.Models
{
    public class LoginUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
