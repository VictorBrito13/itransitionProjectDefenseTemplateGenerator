using System.ComponentModel.DataAnnotations.Schema;

namespace ItransitionTemplates.Models
{
    public class QuestionOption {
        public ulong QuestionOptionId { get; set; }
        public string Option { get; set; }
        [ForeignKey("Question")]
        public ulong QuestionId { get; set; }

        public override string ToString()
        {
            return $"{{ QuestionOptionId: {QuestionOptionId}, Option: {Option}, QuestionId: {QuestionId} }}";
        }
    }
}