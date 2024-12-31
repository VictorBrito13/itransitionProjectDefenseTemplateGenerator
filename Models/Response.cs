using System.ComponentModel.DataAnnotations.Schema;

namespace ItransitionTemplates.Models
{
    [Table("Responses")]
    public class Response {
        public ulong ResponseId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string ResponseString { get; set; }
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        [ForeignKey("Question")]
        public ulong QuestionId { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
}