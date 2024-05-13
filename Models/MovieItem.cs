using System.ComponentModel.DataAnnotations;

namespace MovieAppBackend.Models
{
    public class MovieItem
    {
        [Key]
        public int Id { get; set; }  
        public string Title { get; set; }
        public float IMDBRating { get; set; }
        public string Genres { get; set; }    
        public DateTime ScreeningDate { get; set; }
        public string Description { get; set; }
        public List<Screen> screens { get; set; }
    }
}

//code explanation
//public long Id { get; set; } ==> this code line will automatically create a private
//variable for storing ID value. And asign and read the value by using getters and setters
//No need to manually create a vairable and do that
//

//public class id
//{
//    private long _id;

//    public long getid()
//    {
//        return _id;
//    }

//    public void setid(long id)
//    {
//        _id = id;
//    }

//}

//No need to write all above code lines(26-40)
