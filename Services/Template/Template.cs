using ItransitionTemplates.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Services.Template
{
    public class Template : ITemplate {
        private readonly ApplicationDBContext _context;

        public Template(ApplicationDBContext context) {
            _context = context;
        }

        //Save a template in the database
        public async Task<Models.Template> AddTemplate(Models.Template template) {
            await _context.AddAsync(template);
            var n = await _context.SaveChangesAsync();

            Console.WriteLine(n);
            if(n >= 1) {
                return template;
            }
            return null;
        }

        //Get All templates
        public async Task<Models.Template[]> GetLatestTemplatesWithAdmins(int page, int limit) {
            Models.Template[] templates = await _context.Templates.OrderByDescending(t => t.TemplateId).Include(t => t.Admins).ThenInclude(a => a.User)
            .Skip(page * limit)
            .Take(limit).ToArrayAsync();
            return templates;
        }

        //Get templates by user
        public async Task<Models.Template[]> GetTemplatesByUserId(int page, int limit, ulong userId) {
            Models.Template[] templates = await _context.Templates.Include(t => t.Admins).Where(t => t.Admins.Any(a => a.UserId == userId))
            .Skip(page * limit)
            .Take(limit).ToArrayAsync();
            return templates;
        }
    }
}