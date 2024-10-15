namespace ItransitionTemplates.Services.Question
{
    public interface IQuestion {
        Task<Models.Question[]> AddQuestions(Models.Question[] questions);
    }
}