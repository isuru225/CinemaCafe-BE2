using MovieAppBackend.Frontend.Models;

namespace MovieAppBackend.IServices
{
    public interface IUserService
    {
        public Task<string> AddUnregisteredUsers(int movieId);
        public Task<TokenResult> AddRegisteredUser(RegisterInfo registerInfo);
        public Task<TokenResult> Login(LoginUser loginUser);
        public Task<Profile> GetProfileInfo(string email);
    }
}
