namespace MovieAppBackend.IServices
{
    public interface IUserService
    {
        public Task<string> AddUnregisteredUsers(int movieId);
    }
}
