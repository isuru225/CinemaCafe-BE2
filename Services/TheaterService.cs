using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using MovieAppBackend.Models;

namespace MovieAppBackend.Services
{
    public class TheaterService:ITheater
    {
        private readonly ILogger<TheaterService> _logger;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;
        public TheaterService(ILogger<TheaterService> logger,IConfiguration configaration) 
        {
            _logger = logger;
            _configuration = configaration;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
        }

        public async Task<List<ShowTimeDetails>> GetShowTime(int movieId) 
        {
            var query = """
                    select * 
                    from Screening s
                    inner join Screen sc ON sc.Id = s.ScreenId
                    inner join Theaters t ON t.Id = sc.TheaterId
                    where s.MovieId = @Id
                """;

            List<ShowTimeDetails> details = new List<ShowTimeDetails>();

            Dictionary<(int, int, DateTime) , Screening> showTimeDictionary = new Dictionary<(int,int, DateTime) , Screening>();

            try
            {
                var showTimeDetails = await _sqlConnection.QueryAsync<Screening, Screen, Theater, Screening>(
                            query,
                            (screening, screen, theater) =>
                            {
                                var key = (screening.MovieId, screening.ScreenId, screening.ShowTime);
                                if (!showTimeDictionary.TryGetValue(key, out Screening showTimeDetailsEntry))
                                {
                                    showTimeDetailsEntry = screening;
                                    showTimeDetailsEntry.screen = screen;
                                    showTimeDetailsEntry.screen.theater = theater;

                                    showTimeDictionary.Add(key, showTimeDetailsEntry);
                                }

                                return showTimeDetailsEntry;
                            },
                            new { Id = movieId },
                            splitOn: "Id,Id" // Splitting on "Id" for Movie and Screening
                        );

                
                List<int> theaterIds = new List<int>();
                foreach (var showing in showTimeDetails)
                {
                    ShowTimeDetails showTime = new ShowTimeDetails();
                    showTime.ShowTime = new List<DateTime>();
                    showTime.Experience = new List<string>();

                    if (!theaterIds.Contains(showing.screen.TheaterId))
                    {
                        showTime.TheaterId = showing.screen.TheaterId;
                        showTime.TheaterName = showing.screen.theater.TheaterName;
                        showTime.Location = showing.screen.theater.Location;
                        showTime.ShowTime.Add(showing.ShowTime);
                        showTime.Experience.Add(showing.screen.Experience);
                        theaterIds.Add(showing.screen.TheaterId);
                        
                        details.Add(showTime);
                    }
                    else 
                    {

                        foreach (var detail in details) 
                        {
                            if (showing.screen.TheaterId == detail.TheaterId)
                            {

                                if (!detail.ShowTime.Contains(showing.ShowTime))
                                {
                                    detail.ShowTime.Add(showing.ShowTime);
                                }

                                if (!detail.Experience.Contains(showing.screen.Experience))
                                {
                                    detail.Experience.Add(showing.screen.Experience);
                                }
                            }
                        
                        }
                    }               
                }
                return details;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while quering the data base.");
                return details;
            }

        }

        public async Task<List<Theater>> GetTheaters() 
        {
            List<Theater> theaters = new List<Theater>();

            var query = """
                    select *
                    from Theaters
                """;

            try 
            {
                var results = await _sqlConnection.QueryAsync<Theater>(
                        query
                    );

                foreach (var result in results) 
                {
                    Theater theater = new Theater();
                    theater.TheaterName = result.TheaterName;
                    theater.Location = result.Location;
                    theater.Id = result.Id;

                    theaters.Add( theater );   
                }

                return theaters;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "An error occured while getting the details from the theater table.");
                return theaters;
            }
        }

        public async Task<List<Experience>> GetAllExperiences() 
        {
            List<Experience> theaterExperiences = new List<Experience>();
            
            var query = """
                        SELECT *
                        FROM Experience
                """;

            try 
            {
                var result = await _sqlConnection.QueryAsync<Experience>(
                             query
                             );
                foreach (var experience in result)
                {
                    Experience theaterExperience = new Experience();

                    theaterExperience.Id = experience.Id;
                    theaterExperience.ExperienceType = experience.ExperienceType;

                    theaterExperiences.Add(theaterExperience);
                }

                return theaterExperiences;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting data from the theater experinces table");
                return theaterExperiences;
            }
        }
    }
}
