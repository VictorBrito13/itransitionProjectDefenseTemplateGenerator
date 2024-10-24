using ItransitionTemplates.Data;

namespace ItransitionTemplates.Services.Admin
{
    public class Admin : IAdmin {
        private ApplicationDBContext _context;

        public Admin(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Models.Admin> AddAdmin(Models.Admin admin) {
            await _context.Admins.AddAsync(admin);
            int n = await _context.SaveChangesAsync();

            if(n >= 1) {
                return admin;
            }

            return null;
        }
    }
}