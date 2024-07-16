using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MovieAppBackend.IServices;
using MovieAppBackend.ConfigModels;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public object GetToken(int movieId) 
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

            // Add new claims
            foreach (var claim in claims)
            {
                existingClaims.Add(new Claim(claim.Key, claim.Value));
            }

            // Create a new token with updated claims
            var newTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(existingClaims),
                Expires = DateTime.UtcNow.AddDays(_jWTSettings.ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jWTSettings.Issuer,
                Audience = _jWTSettings.Audience
            };

            var newToken = tokenHandler.CreateToken(newTokenDescriptor);
            var jwt_token = tokenHandler.WriteToken(newToken);


            return new { data = jwt_token };
        }
    }
}
