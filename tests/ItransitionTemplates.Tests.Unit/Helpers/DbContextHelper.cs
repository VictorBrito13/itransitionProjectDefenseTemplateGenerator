using ItransitionTemplates.Data;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Tests.Unit.Helpers
{
    public static class DbContextHelper
    {
        public static ApplicationDBContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDBContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
