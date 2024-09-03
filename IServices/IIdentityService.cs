using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieAppBackend.IServices
{
    public interface IIdentityService
    {
        //private static JwtSecurityTokenHandler TokenHandler => new();
        public SecurityToken CreateSecurityToken(ClaimsIdentity claimsIdentity);
        public string WriteToken(SecurityToken securityToken);
    }
}
