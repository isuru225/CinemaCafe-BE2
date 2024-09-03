namespace MovieAppBackend.Frontend.Models
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? ExpiresOn { get; set; }

        public TokenResult(string _AccessToken, string? _RefreshToken = null, DateTimeOffset? _ExpiresOn = null) 
        {
            AccessToken = _AccessToken;
            RefreshToken = _RefreshToken;
            ExpiresOn = _ExpiresOn;
        }
    }
}
