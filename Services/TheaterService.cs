using Dapper;
using Microsoft.Data.SqlClient;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using MovieAppBackend.Models;
using System.Linq;

namespace MovieAppBackend.Services
{
    public class TheaterService : ITheater
    {
        private readonly ILogger<TheaterService> _logger;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;
        public TheaterService(ILogger<TheaterService> logger, IConfiguration configaration)
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

            Dictionary<(int, int, DateTime), Screening> showTimeDictionary = new Dictionary<(int, int, DateTime), Screening>();

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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public async Task<List<ShowTimeDetails2>> GetShowTime2(int movieId)
        {
            var query = """
                    select * 
                    from Screening s
                    inner join Screen sc ON sc.Id = s.ScreenId
                    inner join Theaters t ON t.Id = sc.TheaterId
                    where s.MovieId = @Id
                """;

            List<ShowTimeDetails2> showTimeDetails2 = new List<ShowTimeDetails2>();
            List<ShowTimes> showTimes = new List<ShowTimes>();

            Dictionary<(int, int, DateTime), Screening> showTimeDictionary = new Dictionary<(int, int, DateTime), Screening>();
            ShowTimeDetails2 showTimeDetail2 = new ShowTimeDetails2();
            showTimeDetail2.theaterInfo = new List<TheaterInfo>();
            try
            {
                var result = await _sqlConnection.QueryAsync<Screening, Screen, Theater, Screening>(
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

                //showTime.theaterInfo.Find(theater => theater.Id == showing.screen.TheaterId)
                List<int> insertedMovieIds = new List<int>();
                List<int> theaterIds = new List<int>();
                List<int> screenIds = new List<int>();

                
                foreach (var showing in result)
                {
                    //check whether the theaterId exists or not.
                    if (theaterIds.Contains(showing.screen.TheaterId))
                    {
                        //FirstOrDefault is used. Becasue only one item is there.
                        TheaterInfo theaterInfo = showTimeDetail2.theaterInfo.Find(theater => theater.Id == showing.screen.TheaterId);
                        int selectedTheaterInfoIndex = showTimeDetail2.theaterInfo.FindIndex(theater => theater.Id == showing.screen.TheaterId);
                        if (screenIds.Contains(showing.ScreenId))
                        {
                            ScreenInfo screenInfo = theaterInfo.screensInfo.Find(screen => screen.Id == showing.ScreenId);
                            int selectedScreenInfoIndex = theaterInfo.screensInfo.FindIndex(screen => screen.Id == showing.ScreenId);
                            showTimeDetail2?.theaterInfo[selectedTheaterInfoIndex].screensInfo[selectedScreenInfoIndex].showTimes.Add(showing.ShowTime);
                        }
                        else
                        {
                            
                            //Enter details into the showTimeDetails2
                            ScreenInfo screenInfo = new ScreenInfo();
                            screenInfo.showTimes = new List<DateTime>();
                            screenInfo.Id = showing.ScreenId;
                            screenInfo.TheaterId = showing.screen.TheaterId;
                            screenInfo.Experience = showing.screen.Experience;
                            screenInfo.showTimes.Add(showing.ShowTime);

                            //access the correct theater object by using the index.
                            showTimeDetail2?.theaterInfo[selectedTheaterInfoIndex].screensInfo.Add(screenInfo);
                            //update the screenIds list.
                            screenIds.Add(showing.ScreenId);
                        }                    

                    }
                    else
                    {
                        
                        showTimeDetail2.MovieId = showing.MovieId;
                        //Create new TheaterInfo object
                        TheaterInfo theaterInfo = new TheaterInfo();
                        //Create new ScreenInfo object
                        theaterInfo.screensInfo = new List<ScreenInfo>();

                        theaterInfo.Id = showing.screen.TheaterId;
                        theaterInfo.TheaterName = showing.screen.theater.TheaterName;
                        theaterInfo.Location = showing.screen.theater.Location;
                        

                        ScreenInfo screenInfo = new ScreenInfo();
                        screenInfo.showTimes = new List<DateTime>();
                        screenInfo.Id = showing.ScreenId;
                        screenInfo.TheaterId = showing.screen.TheaterId;
                        screenInfo.Experience = showing.screen.Experience;
                        screenInfo.showTimes.Add(showing.ShowTime);
                        
                        theaterInfo.screensInfo.Add( screenInfo );
                        //Added theaterInfo details into showTimeDetails
                        showTimeDetail2.theaterInfo.Add(theaterInfo);

                        //update the theaterIds list.
                        theaterIds.Add(showing.screen.TheaterId);
                        //update the screenIds list.
                        screenIds.Add(showing.ScreenId);

                    }
                }
                showTimeDetails2.Add(showTimeDetail2);
                return showTimeDetails2;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while quering the data base.");
                return showTimeDetails2;
            }

        }

        public async Task<List<ShowTimeDetails2>> GetShowTimeForAllMovies()
        {
            var query = """
        select * 
        from Screening s
        inner join Screen sc ON sc.Id = s.ScreenId
        inner join Theaters t ON t.Id = sc.TheaterId
    """;

            List<ShowTimeDetails2> showTimeDetails2 = new List<ShowTimeDetails2>();
            List<ShowTimes> showTimes = new List<ShowTimes>();

            Dictionary<(int, int, DateTime), Screening> showTimeDictionary = new Dictionary<(int, int, DateTime), Screening>();

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
             splitOn: "Id,Id" // Splitting on "Id" for Screening and Screen
         );


                List<int> insertedMovieIds = new List<int>();
                List<int> theaterIds = new List<int>();
                List<int> screenIds = new List<int>();

                foreach (var showing in showTimeDetails)
                {
                    if (insertedMovieIds.Contains(showing.MovieId))
                    {
                        ShowTimeDetails2 showTimeDetail = showTimeDetails2?.Find(showTimeDetail => showTimeDetail.MovieId == showing.MovieId);
                        int selectedshowTimeDetailIndex = showTimeDetails2.FindIndex(showTimeDetail => showTimeDetail.MovieId == showing.MovieId);

                        //ShowTimeDetails2 showTimeDetail2 = new ShowTimeDetails2();
                        //showTimeDetail2.theaterInfo = new List<TheaterInfo>();

                        if (showTimeDetail.theaterInfo.Any(theater => theater.Id == showing.screen.TheaterId))
                        {
                            
                            TheaterInfo theaterInfo = showTimeDetails2[selectedshowTimeDetailIndex].theaterInfo.Find(theater => theater.Id == showing.screen.TheaterId);
                            int selectedTheaterInfoIndex = showTimeDetails2[selectedshowTimeDetailIndex].theaterInfo.FindIndex(theater => theater.Id == showing.screen.TheaterId);
                            if (theaterInfo.screensInfo.Any(screen => screen.Id == showing.ScreenId))
                            {
                                ScreenInfo screenInfo = theaterInfo.screensInfo.Find(screen => screen.Id == showing.ScreenId);
                                int selectedScreenInfoIndex = theaterInfo.screensInfo.FindIndex(screen => screen.Id == showing.ScreenId);
                                showTimeDetails2[selectedshowTimeDetailIndex].theaterInfo[selectedTheaterInfoIndex].screensInfo[selectedScreenInfoIndex].showTimes.Add(showing.ShowTime);
                            }
                            else
                            {

                                //Enter details into the showTimeDetails2
                                ScreenInfo screenInfo = new ScreenInfo();
                                screenInfo.showTimes = new List<DateTime>();
                                screenInfo.Id = showing.ScreenId;
                                screenInfo.TheaterId = showing.screen.TheaterId;
                                screenInfo.Experience = showing.screen.Experience;
                                screenInfo.showTimes.Add(showing.ShowTime);

                                //access the correct theater object by using the index.
                                showTimeDetails2[selectedshowTimeDetailIndex].theaterInfo[selectedTheaterInfoIndex].screensInfo.Add(screenInfo);
                                //update the screenIds list.
                                screenIds.Add(showing.ScreenId);
                            }

                        }
                        else
                        {
                            //Create new TheaterInfo object
                            TheaterInfo theaterInfo = new TheaterInfo();
                            //Create new ScreenInfo object
                            theaterInfo.screensInfo = new List<ScreenInfo>();

                            theaterInfo.Id = showing.screen.TheaterId;
                            theaterInfo.TheaterName = showing.screen.theater.TheaterName;
                            theaterInfo.Location = showing.screen.theater.Location;


                            ScreenInfo screenInfo = new ScreenInfo();
                            screenInfo.showTimes = new List<DateTime>();
                            screenInfo.Id = showing.ScreenId;
                            screenInfo.TheaterId = showing.screen.TheaterId;
                            screenInfo.Experience = showing.screen.Experience;
                            screenInfo.showTimes.Add(showing.ShowTime);

                            theaterInfo.screensInfo.Add(screenInfo);
                            //Added new theaterInfo details into the selected showTimeDetails Object
                            showTimeDetails2[selectedshowTimeDetailIndex].theaterInfo.Add(theaterInfo);

                            //update the theaterIds list.
                            theaterIds.Add(showing.screen.TheaterId);
                            //update the screenIds list.
                            screenIds.Add(showing.ScreenId);
                        }
                      
                    }
                    else
                    {
                        ShowTimeDetails2 showTimeDetail2 = new ShowTimeDetails2();
                        showTimeDetail2.theaterInfo = new List<TheaterInfo>();
                      
                        showTimeDetail2.MovieId = showing.MovieId;
                        
                        //added details into theaterInfo object

                        TheaterInfo theaterInfo = new TheaterInfo();
                        theaterInfo.screensInfo = new List<ScreenInfo>();
                        theaterInfo.Id = showing.screen.TheaterId;
                        theaterInfo.TheaterName = showing.screen.theater.TheaterName;
                        theaterInfo.Location = showing.screen.theater.Location;

                        //added details into screenInfo object

                        ScreenInfo screenInfo = new ScreenInfo();
                        screenInfo.showTimes = new List<DateTime>();

                        screenInfo.Id = showing.ScreenId;
                        screenInfo.TheaterId = showing.screen.TheaterId;
                        screenInfo.Experience = showing.screen.Experience;
                        screenInfo.showTimes.Add(showing.ShowTime);

                        //added ScreenInfo object into TheaterInfo object.

                        theaterInfo.screensInfo.Add(screenInfo);
                        //update ScreenIds list
                        screenIds.Add(showing.ScreenId);

                        //added TheaterInfo object into ShowTimeDetail object.

                        showTimeDetail2.theaterInfo.Add(theaterInfo);
                        //update TheaterIds list
                        theaterIds.Add(showing.screen.TheaterId);

                        //added ShowTimeDetail object into ShowTimeDetails object.
                        showTimeDetails2.Add(showTimeDetail2);
                        //update the insertedMovieIds list
                        insertedMovieIds.Add(showing.MovieId);

                    }
                    
                }
                return showTimeDetails2;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while quering the data base.");
                return showTimeDetails2;
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

                    theaters.Add(theater);
                }

                return theaters;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting data from the theater experinces table");
                return theaterExperiences;
            }
        }
    }
}
