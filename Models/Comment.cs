using System.ComponentModel.DataAnnotations.Schema;

namespace ItransitionTemplates.Models
{
    public class Comment {
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        [ForeignKey("Template")]
        public ulong TemplateId { get; set; }

        public User User { get; set; }
    }
}