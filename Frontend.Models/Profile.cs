namespace MovieAppBackend.Frontend.Models
{
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public List<string> UserRole { get; set; }
    }
}
