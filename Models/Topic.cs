using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Models
{
    [Index(nameof(TopicId), nameof(Name), IsUnique = true)]
    public class Topic {
        public ulong TopicId { get; set; }
        public string Name { get; set; }
        [Required]
        public ICollection<Template> Templates { get; set; }
    }
}