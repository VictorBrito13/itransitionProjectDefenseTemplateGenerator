namespace ItransitionTemplates.Services.Response
{
    public interface IResponse {
        public Task<int> AddResponses(Models.Response[] responses);
    }
}