using ItransitionTemplates.Data;

namespace ItransitionTemplates.Services.Question
{
    public class Question : IQuestion {
        private readonly ApplicationDBContext _context;

        public Question(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Models.Question[]> AddQuestions(Models.Question[] questions) {
            await _context.AddRangeAsync(questions);
            int n = await _context.SaveChangesAsync();

            Console.WriteLine(n);

            if(n >= 1) {
                return questions;
            }

            return null;
        }
    }
}