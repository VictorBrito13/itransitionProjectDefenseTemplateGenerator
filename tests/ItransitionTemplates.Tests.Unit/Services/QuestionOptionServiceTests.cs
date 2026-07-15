using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Tests.Unit.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using QuestionOptionService = ItransitionTemplates.Services.QuestionOption.QuestionOption;

namespace ItransitionTemplates.Tests.Unit.Services
{
    public class QuestionOptionServiceTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly QuestionOptionService _service;

        public QuestionOptionServiceTests()
        {
            _context = DbContextHelper.GetInMemoryDbContext();
            _service = new QuestionOptionService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<Question> SeedQuestion()
        {
            var topic = new Topic { Name = "Test Topic" };
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            var template = new ItransitionTemplates.Models.Template
            {
                Title = "Test Template",
                Description = "Description",
                TopicId = topic.TopicId
            };
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();

            var question = new Question
            {
                QuestionString = "Pick a color",
                TemplateId = template.TemplateId,
                QuestionType = QuestionType.multipleOptions
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        [Fact]
        public async Task AddOptions_ValidOptions_ReturnsOptions()
        {
            // Arrange
            var question = await SeedQuestion();
            var options = new QuestionOption[]
            {
                new QuestionOption { Option = "Red", QuestionId = question.QuestionId },
                new QuestionOption { Option = "Blue", QuestionId = question.QuestionId },
                new QuestionOption { Option = "Green", QuestionId = question.QuestionId }
            };

            // Act
            var result = await _service.AddOptions(options);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.All(result, o => Assert.NotEqual(0UL, o.QuestionOptionId));
        }

        [Fact]
        public async Task AddOptions_EmptyArray_ReturnsNull()
        {
            // Arrange
            var options = Array.Empty<QuestionOption>();

            // Act
            var result = await _service.AddOptions(options);

            // Assert
            Assert.Null(result);
        }
    }
}
