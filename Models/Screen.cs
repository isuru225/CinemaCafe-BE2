namespace MovieAppBackend.Models
{
    public class Screen
    {
        public int Id { get; set; }
        public int TheaterId { get; set; }
        public string Experience { get; set; }
        public List<MovieItem> Movies { get; set; }
        public Theater theater { get; set; }
    }
}
