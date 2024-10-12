using System.ComponentModel.DataAnnotations.Schema;

namespace ItransitionTemplates.Models
{
    public class Question {
        public ulong QuestionId { get; set; }
        public string QuestionString { get; set; }
        [ForeignKey("Template")]
        public ulong TemplateId { get; set; }
        public ICollection<QuestionOption> QuestionOptions { get; set; }
        public Response Response { get; set; }
    }
}