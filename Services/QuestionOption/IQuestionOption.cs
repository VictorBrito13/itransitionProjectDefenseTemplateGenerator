namespace ItransitionTemplates.Services.QuestionOption
{
    public interface IQuestionOption {
        Task<Models.QuestionOption[]> AddOptions(Models.QuestionOption[] questionOptions);
    }
}