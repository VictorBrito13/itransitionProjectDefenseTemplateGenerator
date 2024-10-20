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

        //Get a template by its Id
        public async Task<Models.Template> GetTemplateById(ulong templateId) {

            Models.Template? template = await _context.Templates
            .Include(t => t.Questions)
            .ThenInclude(q => q.QuestionOptions)
            .Include(t => t.Admins)
            .ThenInclude(a => a.User)
            .Include(t => t.usersAllowedToAnswer)
            .ThenInclude(ua => ua.User)
            .Where(t => t.TemplateId == templateId)
            .FirstOrDefaultAsync();

            return template;
        }

        //Update the template
        public async Task<int> UpdateTemplate(ulong templateId, Models.Template template) {
            Models.Template found = await GetTemplateById(templateId);

            if(found == null) return 404;

            //Properties updates
            found.Title = template.Title;
            found.Description = template.Description;
            found.TopicId = template.TopicId;
            found.Admins = template.Admins;
            found.usersAllowedToAnswer = template.usersAllowedToAnswer;
            found.IsPublic = template.IsPublic;
            found.Questions = template.Questions;
            found.Image_url = template.Image_url;

            int n = _context.SaveChanges();

            if(n >= 1) {
                return 200;
            }

            return 500;
        }
    }
}