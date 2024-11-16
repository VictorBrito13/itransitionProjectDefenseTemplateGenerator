using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItransitionTemplates.Models
{
    [Table("Tags")]
    public class Tag {
        public ulong TagId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<Template> Templates { get; set; }
    }
}