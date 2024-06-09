using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class Experience
    {
        [Key]
        public int Id { get; set; }
        public string ExperienceType { get; set; }
    }
}
