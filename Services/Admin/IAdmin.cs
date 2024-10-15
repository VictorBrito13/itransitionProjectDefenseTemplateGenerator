namespace ItransitionTemplates.Services.Admin
{
    public interface IAdmin {
        Task<Models.Admin> AddAdmin(Models.Admin admin);
    }
}