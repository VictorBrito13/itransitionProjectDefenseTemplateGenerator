using System.Runtime.CompilerServices;

namespace ItransitionTemplates.Services.User
{
    public interface IUserService {
        Task<string> AddUser(Models.User user);
        Task<Models.User> Login(Models.User email);
        Task<Models.User> GetUserByUsername(string username);
    }
}