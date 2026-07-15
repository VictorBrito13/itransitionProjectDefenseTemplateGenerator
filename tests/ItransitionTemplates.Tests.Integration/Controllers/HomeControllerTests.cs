using System.Net;
using System.Text.Json;
using Xunit;

namespace ItransitionTemplates.Tests.Integration.Controllers;

public class HomeControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HomeControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetIndex_ReturnsOkWithHtml()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString() ?? "");
    }

    [Fact]
    public async Task GetError_ReturnsOkWithHtml()
    {
        // Act
        var response = await _client.GetAsync("/Home/Error");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString() ?? "");
    }
}
