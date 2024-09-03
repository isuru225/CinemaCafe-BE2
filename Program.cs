
using Microsoft.EntityFrameworkCore;
using MovieAppBackend.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieAppBackend.IServices;
using MovieAppBackend.Services;
using MovieAppBackend.ConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace MovieAppBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddDbContext<MovieAppDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("MovieAppDbContext") ?? throw new InvalidOperationException("Connection string 'MovieAppDbContext' not found.")));


            builder.Services.AddDbContext<IdentityDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("MovieAppDbContext") ?? throw new InvalidOperationException("Connection string 'MovieAppDbContext' not found.")));

            // Add services to the container.

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddSignInManager();

            // Configure Identity options
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Adjust origin as needed
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials(); // Allow credentials (cookies)
                });
            });

            //JWT

            var jwtSettings = new JWTSettings();
            builder.Configuration.Bind("Jwt", jwtSettings);

            // Configure JWTSettings for DI
            var jwtSection = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JWTSettings>(jwtSection);


            //string key = "secret_key_12345_Auth_&_Author_$10010010_##"; //this should be same which is used while creating token      
            //var issuer = "http://localhost:3000/";  //this should be same which is used while creating token 

            builder.Services.AddAuthentication(a => 
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key ?? 
                    throw new InvalidOperationException())),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,      
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    RequireExpirationTime = false
                };
                options.Audience = jwtSettings.Audience;
                options.ClaimsIssuer = jwtSettings.Issuer;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                };
            });



            ///////////////////
            ///
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ITheater, TheaterService>();
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISeatPlanService, SeatPlanService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
