using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ItransitionTemplates.Models
{
    [Table("Templates")]
    public class Template {

        public ulong TemplateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image_url { get; set; } = "default.png";
        public bool IsPublic { get; set; } = true;
        [ForeignKey("Topic")]
        public ulong TopicId { get; set; }
        [Required]
        public Topic Topic { get; set; }
        public ICollection<Like> Likes { get; set; } = [];
        public ICollection<Admin> Admins { get; set; } = [];
        public ICollection<Tag> Tags { get; set; } = [];
        [Required]
        public ICollection<Question> Questions { get; set; } = [];
        public ICollection<UserAllowedToAnswer> usersAllowedToAnswer { get; set; } = [];
    }
}