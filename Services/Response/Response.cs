using ItransitionTemplates.Data;

namespace ItransitionTemplates.Services.Response
{
    public class Response : IResponse {
        private ApplicationDBContext _context;

        public Response(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<int> AddResponses(Models.Response[] responses) {
            await _context.AddRangeAsync(responses);

            int n = _context.SaveChanges();

            if(n >= 1) {
                return 201;
            }

            return 403;
        }
    }
}