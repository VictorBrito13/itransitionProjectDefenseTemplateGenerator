using System.ComponentModel.DataAnnotations.Schema;
using ItransitionTemplates.Data;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Services.Topic
{
    [Table("topics")]
    public class Topic : ITopic {
        private readonly ApplicationDBContext _context;

        public Topic(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Models.Topic[]> GetTopics() {
            return await _context.Topics.ToArrayAsync();
        }
    }
}