using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieAppBackend.Custom.Exceptions;
using MovieAppBackend.Frontend.Models;
using MovieAppBackend.IServices;
using Microsoft.AspNetCore.Identity;
using System.Web.Http.ModelBinding;
using MovieAppBackend.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;

namespace MovieAppBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _sqlConnection;
        ILogger<UserService> _logger;
        private IMovieService _movieService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IIdentityService _identityService;
        public UserService
        (
            IConfiguration configuration,
            ILogger<UserService> logger,
            IMovieService movieService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IIdentityService identityService
        )
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration.GetConnectionString("MovieAppDbContext"));
            _logger = logger;
            _movieService = movieService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _identityService = identityService;
        }

        public async Task<TokenResult> AddRegisteredUser(RegisterInfo registerInfo)
        {

            //No need to add Id here, userManage class will automatically add GUID when createAsync is called.
            var user = new IdentityUser
            {
                UserName = registerInfo.Email,
                Email = registerInfo.Email,
                PhoneNumber = registerInfo.MobileNumber
            };

            // Store user data in AspNetUsers database table
            try
            {
                var result = await _userManager.CreateAsync(user, registerInfo.Password);
                // add First Name and last name as claims
                var newClaims = new List<Claim>
                {
                    new("FirstName", registerInfo.FirstName),
                    new("LastName", registerInfo.LastName),
                    new("UserId", user?.Id)
                };
                //add new claims
                try
                {
                    await _userManager.AddClaimsAsync(user, newClaims);
                    //add roles 
                    try
                    {
                        var role = await _roleManager.FindByNameAsync("User");
                        if (role == null)
                        {
                            role = new IdentityRole("User");
                            try
                            {
                                await _roleManager.CreateAsync(role);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "An error occured while create new role.");
                                throw ex;
                            }

                        }

                        try
                        {
                            await _userManager.AddToRoleAsync(user, "User");
                            //add newly added roles to the claims
                            newClaims.Add(new Claim(ClaimTypes.Role, "User"));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occured while adding the role to the user.");
                            throw ex;
                        }
                        //Create a ClaimsIdentity to be used when generating the JWT
                        var claimsIdentity = new ClaimsIdentity(new Claim[]
                        {
                        new( JwtRegisteredClaimNames.Sub, registerInfo.Email ?? throw new InvalidOperationException()),
                        new( JwtRegisteredClaimNames.Email, registerInfo.Email ?? throw new InvalidOperationException())
                        });

                        //also add first name and last name into the JWT
                        claimsIdentity.AddClaims(newClaims);

                        var token = _identityService.CreateSecurityToken(claimsIdentity);
                        var response = new TokenResult(_identityService.WriteToken(token));

                        return response;

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occured while finding the role name.");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while adding new claims.");
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while registering new user.");
                throw ex;
            }
        }

        public async Task<TokenResult> Login(LoginUser loginUser)
        {
            //verify the user is exist or not
            try
            {   //username and password are same in here.
                var user = await _userManager.FindByEmailAsync(loginUser.UserName);
                if (user == null)
                {
                    throw new UserNotFoundException("Could not find an user by provided email.");
                }

                //verify the combination of email and password is correct.
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
                if (!result.Succeeded)
                {
                    throw new InvalidCredentialException("Could not sign in.");
                }

                //Get the claims from the database

                try
                {
                    var claims = await _userManager.GetClaimsAsync(user);
                    //Get the roles from the database
                    try
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        //create JWT token for the user
                        var claimsIdentity = new ClaimsIdentity(new Claim[]
                        {
                            new(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
                            new(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException()),
                        });

                        //add claims from the data base;
                        claimsIdentity.AddClaims(claims);
                        //add some extra claims
                        var userIdClaim = new List<Claim>
                        {
                            new Claim("userId", user.Id)
                        };
                        claimsIdentity.AddClaims(userIdClaim);
                        //add roles from the data base;
                        foreach (var role in roles)
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }

                        //Generate the token
                        var token = _identityService.CreateSecurityToken(claimsIdentity);

                        //Generate and send the response
                        var response = new TokenResult(_identityService.WriteToken(token));
                        return response;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occured while getting user roles from data base.");
                        throw ex;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while getting the claims from the data base");
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while finding an user by email");
                throw ex;
            }
        }

        public async Task Logout()
        {

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
                throw ex;
            }

        }

        public async Task<Profile> GetProfileInfo(string email) 
        {
            try
            {
                Profile profile = new Profile();
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new UserNotFoundException("Could not find an user by provided email.");
                }

                //Get the claims from the database
                try 
                {
                    var claims = await _userManager.GetClaimsAsync(user);

                    try
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        // add First Name and Last name to the profile modal.
                        foreach (var claim in claims) 
                        {
                            if (claim.Type == "FirstName") 
                            {
                                profile.FirstName = claim.Value;
                            }
                            if (claim.Type == "LastName") 
                            {
                                profile.LastName = claim.Value;    
                            }
                        }

                        //create userRole instance
                        profile.UserRole = new List<string>();
                        foreach (var role in roles) 
                        {
                            profile.UserRole.Add(role);
                        }

                        profile.Email = user?.Email;
                        profile.MobileNumber = user?.PhoneNumber;

                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, "An error occured while getting the user claims.");
                        throw ex;
                    }
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "An error occured while getting the user claims.");
                    throw ex;
                }
                return profile;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occured while findig the email.");
                throw ex;
            }

        }
    }
}
