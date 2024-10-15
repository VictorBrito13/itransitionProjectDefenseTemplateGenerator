using ItransitionTemplates.Data;

namespace ItransitionTemplates.Services.Template
{
    public class Template : ITemplate {
        private readonly ApplicationDBContext _context;

        public Template(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Models.Template> AddTemplate(Models.Template template) {
            await _context.AddAsync(template);
            var n = await _context.SaveChangesAsync();

            Console.WriteLine(n);
            if(n >= 1) {
                return template;
            }
            return null;
        }
    }
}