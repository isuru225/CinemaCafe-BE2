using MovieAppBackend.Frontend.Models;

namespace MovieAppBackend.IServices
{
    public interface ITokenService
    {
        public object GetToken(Home home);
        public object RenewToken(string token,Dictionary<string, string> claims);
    }
}
