using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using ItransitionTemplates.Utils;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AdminService = ItransitionTemplates.Services.Admin.Admin;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class AdminServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _service = new AdminService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<(ItransitionTemplates.Models.User user, ItransitionTemplates.Models.Template template)> SeedUserAndTemplate()
        {
            var topic = new Topic { Name = "Test Topic" };
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            var user = new ItransitionTemplates.Models.User
            {
                Username = "adminuser",
                Email = "admin@example.com",
                Password = HashText.GetHashString("password")
            };
            _context.Users.Add(user);

            var template = new ItransitionTemplates.Models.Template
            {
                Title = "Test Template",
                Description = "Description",
                TopicId = topic.TopicId
            };
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();

            return (user, template);
        }

        [Fact]
        public async Task AddAdmin_ValidAdmin_ReturnsAdmin()
        {
            // Arrange
            var (user, template) = await SeedUserAndTemplate();
            var admin = new ItransitionTemplates.Models.Admin
            {
                UserId = user.UserId,
                TemplateId = template.TemplateId
            };

            // Act
            var result = await _service.AddAdmin(admin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(template.TemplateId, result.TemplateId);
        }

        [Fact]
        public async Task AddAdmin_DuplicateAdmin_ThrowsException()
        {
            // Arrange
            var (user, template) = await SeedUserAndTemplate();
            var admin1 = new ItransitionTemplates.Models.Admin { UserId = user.UserId, TemplateId = template.TemplateId };
            await _service.AddAdmin(admin1);

            var admin2 = new ItransitionTemplates.Models.Admin { UserId = user.UserId, TemplateId = template.TemplateId };

            // Act & Assert
            // InMemory provider detects duplicate key at change tracker level (InvalidOperationException)
            // whereas a real database would throw DbUpdateException at save time
            await Assert.ThrowsAnyAsync<Exception>(() => _service.AddAdmin(admin2));
        }
    }
}
