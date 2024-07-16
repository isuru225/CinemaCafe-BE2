namespace MovieAppBackend.IServices
{
    public interface ITokenService
    {
        public object GetToken(int movieId);
        public object RenewToken(string token,Dictionary<string, string> claims);
    }
}
