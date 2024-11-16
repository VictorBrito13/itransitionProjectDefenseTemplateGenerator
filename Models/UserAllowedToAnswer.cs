using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ItransitionTemplates.Models
{
    [Table("UserAllowedToAnswers")]
    public class UserAllowedToAnswer {
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        [ForeignKey("Template")]
        public ulong TemplateId { get; set; }
        [JsonIgnore]
        public Template Template { get; set; }
        public User User { get; set; }
    }
}