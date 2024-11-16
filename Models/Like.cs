
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ItransitionTemplates.Models {
    [Table("Likes")]
    public class Like {
        public ulong TemplateId { get; set; }
        public ulong UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Template Template { get; set; }
    }
}