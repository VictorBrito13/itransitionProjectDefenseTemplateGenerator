namespace ItransitionTemplates.Services.Topic
{
    public interface ITopic {
        Task<Models.Topic[]> GetTopics();
    }
}