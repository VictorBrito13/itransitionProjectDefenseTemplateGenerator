using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ItransitionTemplates.Models {
    [Table("Admins")]
    public class Admin {
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        [ForeignKey("Template")]
        public ulong TemplateId { get; set; }
        public User User { get; set; }
        [JsonIgnore]
        public Template Template { get; set; }
    }
}