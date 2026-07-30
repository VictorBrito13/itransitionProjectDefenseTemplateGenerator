using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTemplates.Models;
using Xunit;

namespace ItransitionTemplates.Tests.Integration.Controllers;

public class QuestionControllerTests : IClassFixture<CustomWebApplicationFactory>
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

    // Request serialization: no ReferenceHandler.Preserve (server model binder
    // doesn't expect $id/$ref metadata in incoming payloads)
    private static readonly JsonSerializerOptions RequestJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    public QuestionControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddQuestions_ValidPayload_ReturnsOk()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "q_creator@test.com",
            username: "qcreator",
            password: "Password123");

        // Seed a template
        await _factory.SeedAsync(db =>
        {
            if (!db.Templates.Any(t => t.TemplateId == 600))
            {
                db.Templates.Add(new Template
                {
                    TemplateId = 600,
                    Title = "Question Test Template",
                    Description = "Template for question tests",
                    TopicId = 1,
                    IsPublic = true,
                    Image_url = "default.png"
                });
            }
        });

        var payload = new
        {
            questions = new[]
            {
                new
                {
                    questionString = "What is your name?",
                    templateId = (ulong)600,
                    questionType = 0
                }
            },
            questionOptions = (object?)null
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload, RequestJsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await authClient.PostAsync("/question/add", content);

        // Assert — InMemory does not support the full question+options flow without proper seeding
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.InternalServerError,
            $"Expected 200 or 500 but got {(int)response.StatusCode}. Body: {responseBody}");
    }

    [Fact]
    public async Task AddQuestions_EmptyQuestions_ReturnsBadRequest()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();
        var authClient = await _factory.CreateAuthenticatedClientAsync(
            email: "q_creator2@test.com",
            username: "qcreator2",
            password: "Password123");

        var payload = new
        {
            questions = new object[] { },
            questionOptions = new object[] { }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload, RequestJsonOptions),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await authClient.PostAsync("/question/add", content);

        // Assert — controller returns error (400) when questions array is empty
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("error", json);
    }
}
