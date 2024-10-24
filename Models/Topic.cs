using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Models
{
    [Index(nameof(TopicId), nameof(Name), IsUnique = true)]
    public class Topic {
        public ulong TopicId { get; set; }
        public string Name { get; set; }
        [Required]
        [JsonIgnore]
        public ICollection<Template> Templates { get; set; }
    }
}