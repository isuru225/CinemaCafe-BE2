using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieAppBackend.ConfigModels;
using MovieAppBackend.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace MovieAppBackend.Services
{
    public class IdentityService : IIdentityService
    {
        private JWTSettings _jwtSettings;
        private byte[] _key;
        public IdentityService(IOptions<JWTSettings> jwtOtions) 
        {
            //Get JWT values inside the appsettings.json
            _jwtSettings = jwtOtions.Value;
            ArgumentNullException.ThrowIfNull(_jwtSettings);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Issuer);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Audience);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Key);
            _key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        }
        //Helps to create new JWT token
        private static JwtSecurityTokenHandler TokenHandler => new();

        //private JwtSecurityTokenHandler TokenHandler() 
        //{
        //    JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //    return jwtSecurityTokenHandler

        //}

        public SecurityToken CreateSecurityToken(ClaimsIdentity claimsIdentity) 
        {
            var tokenDescriptor = GetTokenDescriptor(claimsIdentity);

            return TokenHandler.CreateToken(tokenDescriptor);
        }

        public string WriteToken(SecurityToken securityToken)
        {
            return TokenHandler.WriteToken(securityToken);
        }
    
        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claimsIdentity) 
        {
            return new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddHours(5),
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
            };
        }

    

    }
}
