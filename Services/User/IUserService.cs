namespace ItransitionTemplates.Services.User
{
    public interface IUserService {
        Task<Models.User> AddUser(Models.User user);
        Task<Models.User> Login(Models.User email);
        Task<Models.User> GetUserByUsername(string username);
    }
}