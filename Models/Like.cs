
using System.Text.Json.Serialization;

namespace ItransitionTemplates.Models {
    public class Like {
        public ulong TemplateId { get; set; }
        public ulong UserId { get; set; }

        public User User { get; set; }
        [JsonIgnore]
        public Template Template { get; set; }
    }
}