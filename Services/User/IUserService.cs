namespace ItransitionTemplates.Services.User
{
    public interface IUserService {
        Task<string> AddUser(Models.User user);
    }
}