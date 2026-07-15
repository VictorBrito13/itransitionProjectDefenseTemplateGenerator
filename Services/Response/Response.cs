using ItransitionTemplates.Data;
using ItransitionTemplates.Models;

namespace ItransitionTemplates.Services.Response
{
    public class Response : IResponse {
        private readonly ApplicationDBContext _context;

        public Response(ApplicationDBContext context) {
            _context = context;
        }

        public async Task AddResponses(Models.Response[] responses) {
            await _context.AddRangeAsync(responses);

            int n = _context.SaveChanges();

            if(n < 1)
                throw new ServiceException("Failed to save responses — please try again", ServiceErrorCode.Database);
        }
    }
}