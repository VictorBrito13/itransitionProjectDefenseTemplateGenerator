using System.ComponentModel.DataAnnotations;

namespace ItransitionTemplates.Models
{
    public class Tag {
        public ulong TagId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<Template> Templates { get; set; }
    }
}