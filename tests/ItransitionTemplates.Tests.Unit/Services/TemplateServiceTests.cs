using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using TemplateService = ItransitionTemplates.Services.Template.Template;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class TemplateServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly TemplateService _service;
        private readonly Microsoft.Extensions.Logging.ILogger<TemplateService> _logger;

        public TemplateServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<TemplateService>.Instance;
            _service = new TemplateService(_context, _logger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<Topic> SeedTopic(string name = "Test Topic")
        {
            var topic = new Topic { Name = name };
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }

        private async Task<ItransitionTemplates.Models.Template> SeedTemplate(ulong topicId, string title = "Test Template", bool isPublic = true)
        {
            var template = new ItransitionTemplates.Models.Template
            {
                Title = title,
                Description = "Test Description",
                TopicId = topicId,
                IsPublic = isPublic
            };
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
            return template;
        }

        [Fact]
        public async Task AddTemplate_ValidTemplate_ReturnsTemplate()
        {
            // Arrange
            var topic = await SeedTopic();
            var template = new ItransitionTemplates.Models.Template
            {
                Title = "New Template",
                Description = "Description",
                TopicId = topic.TopicId
            };

            // Act
            var result = await _service.AddTemplate(template);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Template", result.Title);
        }

        [Fact]
        public async Task AddTemplate_SetsTemplateId()
        {
            // Arrange
            var topic = await SeedTopic();
            var template = new ItransitionTemplates.Models.Template
            {
                Title = "Template With ID",
                Description = "Description",
                TopicId = topic.TopicId
            };

            // Act
            var result = await _service.AddTemplate(template);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0UL, result.TemplateId);
        }

        [Fact]
        public async Task GetLatestTemplatesWithAdmins_ReturnsPublicOnly()
        {
            // Arrange
            var topic = await SeedTopic();
            await SeedTemplate(topic.TopicId, "Public 1", isPublic: true);
            await SeedTemplate(topic.TopicId, "Public 2", isPublic: true);
            await SeedTemplate(topic.TopicId, "Private 1", isPublic: false);

            // Act
            var result = await _service.GetLatestTemplatesWithAdmins(0, 10);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.All(result, t => Assert.True(t.IsPublic));
        }

        [Fact]
        public async Task GetLatestTemplatesWithAdmins_RespectsPagination()
        {
            // Arrange
            var topic = await SeedTopic();
            for (int i = 0; i < 5; i++)
            {
                await SeedTemplate(topic.TopicId, $"Template {i}");
            }

            // Act - Get page 0 with limit 2
            var page0 = await _service.GetLatestTemplatesWithAdmins(0, 2);
            var page1 = await _service.GetLatestTemplatesWithAdmins(1, 2);

            // Assert
            Assert.Equal(2, page0.Length);
            Assert.Equal(2, page1.Length);
            // Pages should contain different templates
            Assert.NotEqual(page0[0].TemplateId, page1[0].TemplateId);
        }

        [Fact]
        public async Task GetTemplateById_WithQuestions_IncludesQuestionsAndOptions()
        {
            // Arrange
            var topic = await SeedTopic();
            var template = await SeedTemplate(topic.TopicId);
            
            var question = new Question
            {
                QuestionString = "What is your name?",
                TemplateId = template.TemplateId,
                QuestionType = QuestionType.singleLineString
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var option = new QuestionOption
            {
                Option = "Option A",
                QuestionId = question.QuestionId
            };
            _context.QuestionOptions.Add(option);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetTemplateById(template.TemplateId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Questions);
            Assert.Single(result.Questions.First().QuestionOptions);
        }

        [Fact]
        public async Task UpdateTemplate_ExistingTemplate_Returns200()
        {
            // Arrange
            var topic = await SeedTopic();
            var template = await SeedTemplate(topic.TopicId);
            var updateData = new ItransitionTemplates.Models.Template
            {
                Title = "Updated Title",
                Description = "Updated Description",
                TopicId = topic.TopicId
            };

            // Act
            var result = await _service.UpdateTemplate(template.TemplateId, updateData);

            // Assert
            Assert.Equal(200, result);
            var updated = await _context.Templates.FindAsync(template.TemplateId);
            Assert.Equal("Updated Title", updated.Title);
        }

        [Fact]
        public async Task UpdateTemplate_NonExistent_Returns404()
        {
            // Arrange
            var updateData = new ItransitionTemplates.Models.Template { Title = "Updated" };

            // Act
            var result = await _service.UpdateTemplate(99999, updateData);

            // Assert
            Assert.Equal(404, result);
        }

        [Fact]
        public async Task DeleteTemplate_ExistingTemplate_Returns200()
        {
            // Arrange
            var topic = await SeedTopic();
            var template = await SeedTemplate(topic.TopicId);

            // Act
            var result = await _service.DeleteTemplate(template.TemplateId);

            // Assert
            Assert.Equal(200, result);
            var deleted = await _context.Templates.FindAsync(template.TemplateId);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task LikeAction_Like_AddsLike()
        {
            // Arrange
            var topic = await SeedTopic();
            var template = await SeedTemplate(topic.TopicId);
            var userId = 1UL;

            // Act
            var result = await _service.LikeAction(userId, template.TemplateId, "like");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(userId, result[0].UserId);
            Assert.Equal(template.TemplateId, result[0].TemplateId);
        }

        [Fact]
        public async Task GetTemplatesByQuery_ThrowsWithInMemoryProvider()
        {
            // Arrange
            var topic = await SeedTopic();
            await SeedTemplate(topic.TopicId, "Searchable Template");

            // Act & Assert
            // InMemory provider does not support FromSqlRaw
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.GetTemplatesByQuery("Searchable"));
        }
    }
}
