using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieAppBackend.Custom.Exceptions;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _sqlConnection;
        ILogger<UserService> _logger;
        private IMovieService _movieService;
        public UserService(IConfiguration configuration,ILogger<UserService> logger,IMovieService movieService) 
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
            _logger = logger;
            _movieService = movieService;
        }

        public async Task<string> AddUnregisteredUsers(int movieId) 
        {
            var movieRecords = await _movieService.GetMovieForGivenId(movieId);
            if (movieRecords == null) 
            {
                throw new DataNotFoundException("Requested movie id is not available");
            }

            var query = """
                            INSERT INTO USERS (Id,UserName,UserEmail,MobileNumber,IsRegistered) VALUES (@Id,@UserName,@UserEmail,@MobileNumber,@IsRegistered);SELECT CAST(SCOPE_IDENTITY() as int);
                """;
            var userId = Guid.NewGuid().ToString();
            
            try
            {
                var result = await _sqlConnection.QuerySingleAsync<string>(
                        query,
                new UnregisteredUser
                { 
                    Id = userId,
                    UserName = null,
                    UserEmail = null,
                    MobileNumber = null,
                    IsRegistered = false
                }
                );
                return userId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding user id into Users table.");
                throw;
            }

        }
    }
}
