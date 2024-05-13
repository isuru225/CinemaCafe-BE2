
using Microsoft.EntityFrameworkCore;
using MovieAppBackend.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieAppBackend.IServices;
using MovieAppBackend.Services;

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //session
            //builder.Services.AddDistributedMemoryCache();

            //builder.Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(60);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.Name = ".AdventureWorks.Session";
            //    options.Cookie.IsEssential = true;
            //});

            // configure CORS for single domain
            //builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
            //{
            //    build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            //}));

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

            string key = "secret_key_12345_Auth_&_Author_$10010010_##"; //this should be same which is used while creating token      
            var issuer = "http://localhost:3000/";  //this should be same which is used while creating token 

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

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
            builder.Services.AddScoped<IMovieService,MovieService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ITheater, TheaterService>();

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
