using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MovieAppBackend.IServices;

namespace MovieAppBackend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public object GetToken(int movieId) 
        {
            string key = "secret_key_12345_Auth_&_Author_$10010010_##"; //Secret key which will be used later during validation    
            var issuer = "http://localhost:3000/";  //normally this will be your site URL    

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //Debug.WriteLine("CVCV",name);
            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            string userID = Guid.NewGuid().ToString();
            permClaims.Add(new Claim("userid", userID));
            permClaims.Add(new Claim("movieId", movieId.ToString()));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }
    }
}
