using ItransitionTemplates.Data;

namespace ItransitionTemplates.Services.Question
{
    public class Question : IQuestion {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<Question> _logger;

        public Question(ApplicationDBContext context, ILogger<Question> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<Models.Question[]> AddQuestions(Models.Question[] questions) {
            await _context.AddRangeAsync(questions);
            int n = await _context.SaveChangesAsync();

            _logger.LogInformation("AddQuestions: {RowsAffected} rows affected", n);

            if(n >= 1) {
                return questions;
            }

            return null;
        }
    }
}