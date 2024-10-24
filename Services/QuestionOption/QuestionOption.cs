using ItransitionTemplates.Data;

namespace ItransitionTemplates.Services.QuestionOption
{
    public class QuestionOption : IQuestionOption {
        private ApplicationDBContext _context;

        public QuestionOption(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Models.QuestionOption[]> AddOptions(Models.QuestionOption[] questionOptions) {
            await _context.QuestionOptions.AddRangeAsync(questionOptions);

            int n = await _context.SaveChangesAsync();

            if(n >= 1) {
                return questionOptions;
            }

            return null;
        }
    }
}