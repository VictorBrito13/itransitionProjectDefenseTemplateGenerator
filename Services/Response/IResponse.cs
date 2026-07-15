namespace ItransitionTemplates.Services.Response
{
    public interface IResponse {
        public Task AddResponses(Models.Response[] responses);
    }
}