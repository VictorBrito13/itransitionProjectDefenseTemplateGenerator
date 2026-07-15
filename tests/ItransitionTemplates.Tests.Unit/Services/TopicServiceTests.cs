using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using TopicService = ItransitionTemplates.Services.Topic.Topic;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class TopicServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly TopicService _service;

        public TopicServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _service = new TopicService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetTopics_SeededTopics_ReturnsAll()
        {
            // Arrange
            _context.Topics.Add(new Topic { Name = "Technology" });
            _context.Topics.Add(new Topic { Name = "Science" });
            _context.Topics.Add(new Topic { Name = "Art" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetTopics();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Contains(result, t => t.Name == "Technology");
            Assert.Contains(result, t => t.Name == "Science");
            Assert.Contains(result, t => t.Name == "Art");
        }

        [Fact]
        public async Task GetTopics_EmptyDatabase_ReturnsEmptyArray()
        {
            // Act
            var result = await _service.GetTopics();

            // Assert
            Assert.Empty(result);
        }
    }
}
