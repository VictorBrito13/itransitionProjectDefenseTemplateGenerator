using System.ComponentModel.DataAnnotations.Schema;

namespace ItransitionTemplates.Models
{
    public enum QuestionType {
        singleLineString,
        multipleLineText,
        positiveInteger,
        checkBox,
        multipleOptions
    }
    public class Question {
        public ulong QuestionId { get; set; }
        public string QuestionString { get; set; }
        [ForeignKey("Template")]
        public ulong TemplateId { get; set; }
        public QuestionType QuestionType { get; set; }
        public ICollection<QuestionOption>? QuestionOptions { get; set; }
        public Response? Response { get; set; }
    }
}