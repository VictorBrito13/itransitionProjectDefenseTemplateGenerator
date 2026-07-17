using ItransitionTemplates.Data;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Services.Admin
{
    public class Admin : IAdmin {
        private readonly ApplicationDBContext _context;

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

        public async Task<bool> IsUserAdmin(ulong userId, ulong templateId) {
            return await _context.Admins.AnyAsync(a => a.UserId == userId && a.TemplateId == templateId);
        }
    }
}