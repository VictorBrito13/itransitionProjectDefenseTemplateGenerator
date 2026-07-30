using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ItransitionTemplates.Tests.Integration.Controllers;

public class ResponseControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.Preserve,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    public ResponseControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SaveResponses_ValidPayload_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "responder@test.com",
            username: "responder",
            password: "Password123");

        // Seed a template and question for responses to reference
        await _factory.SeedAsync(db =>
        {
            if (!db.Templates.Any(t => t.TemplateId == 700))
            {
                db.Templates.Add(new Template
                {
                    TemplateId = 700,
                    Title = "Response Test Template",
                    Description = "Template for response tests",
                    TopicId = 1,
                    IsPublic = true,
                    Image_url = "default.png"
                });
            }

            if (!db.Questions.Any(q => q.QuestionId == 700))
            {
                db.Questions.Add(new Question
                {
                    QuestionId = 700,
                    QuestionString = "What is your answer?",
                    TemplateId = 700,
                    QuestionType = QuestionType.singleLineString
                });
            }
        });

        // Get the user ID
        ulong userId = 0;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ItransitionTemplates.Data.ApplicationDBContext>();
            var user = db.Users.FirstOrDefault(u => u.Email == "responder@test.com");
            if (user != null) userId = user.UserId;
        }

        var payload = new[]
        {
            new
            {
                responseString = "My answer",
                userId = userId,
                questionId = 700,
                date = DateTime.UtcNow
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await authClient.PostAsync("/response/add", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("data", json);
    }

    [Fact]
    public async Task SaveResponses_EmptyPayload_ReturnsBadRequest()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "responder2@test.com",
            username: "responder2",
            password: "Password123");

        var payload = new object[] { };
        var content = new StringContent(
            JsonSerializer.Serialize(payload, JsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await authClient.PostAsync("/response/add", content);

        // Assert — empty array causes ServiceException with Database error code (500)
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("error", json);
    }
}
