using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MovieAppBackend.IServices;
using MovieAppBackend.ConfigModels;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using MovieAppBackend.Frontend.Models;

namespace MovieAppBackend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly JWTSettings _jWTSettings;
        public TokenService(IConfiguration configuration,IOptions<JWTSettings> options) 
        {
            _configuration = configuration;
            _jWTSettings = options.Value;
        }

        public object GetToken(Home home) 
        {

            //string key = "secret_key_12345_Auth_&_Author_$10010010_##"; //Secret key which will be used later during validation
            string key = _jWTSettings.Key;
            var issuer = _jWTSettings.Issuer;  //normally this will be your site URL    

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //Debug.WriteLine("CVCV",name);
            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            string userID = Guid.NewGuid().ToString();
            permClaims.Add(new Claim("userId", home.UserId));
            permClaims.Add(new Claim("movieId", home.MovieId.ToString()));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }

        public object RenewToken(string existingToken, Dictionary<string, string> claims) 
        { 
            var securityKey = Encoding.UTF8.GetBytes(_jWTSettings.Key);
            var tokenHandler = new JwtSecurityTokenHandler();

            // Define token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            // Validate the existing token and extract the claims
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(existingToken, tokenValidationParameters, out validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var existingClaims = jwtToken.Claims.ToList();

            // Check for existing "aud" claim
            var existingAudClaim = existingClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud && c.Value == "http://localhost:3000/");

            if (existingAudClaim == null)
            {
                // Add the new "aud" claim if it doesn't already exist
                existingClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, _jWTSettings.Audience));
            }

            // Add new claims
            foreach (var claim in claims)
            {
                //existingClaims.Add(new Claim(claim.Key, claim.Value));
                //check the new claim key is already available in the existing token.
                bool claimIsAlreadyAvailable = false;
                for (int x = 0; x < existingClaims.Count(); x++) 
                {
                    var property = existingClaims[x].Type;
                    if (property != null) 
                    {
                        if (property == claim.Key) 
                        {
                            //Add new claim value
                            existingClaims[x] = new Claim(claim.Key, claim.Value);
                            claimIsAlreadyAvailable = true;
                        }  
                    }
                }
                if (!claimIsAlreadyAvailable) 
                {
                    existingClaims.Add(new Claim(claim.Key, claim.Value));
                }

            }

            // Create a new token with updated claims
            var newTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(existingClaims),
                Expires = DateTime.UtcNow.AddDays(_jWTSettings.ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var newToken = tokenHandler.CreateToken(newTokenDescriptor);
            var jwt_token = tokenHandler.WriteToken(newToken);


            return new { data = jwt_token };
        }
    }
}
