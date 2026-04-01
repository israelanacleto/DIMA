using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Dima.Tests.IntegrationTests;

public class HealthCheckTests(DimaWebApplicationFactory factory) : IClassFixture<DimaWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Get_Root_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadFromJsonAsync<dynamic>();
        string message = content?.GetProperty("message").GetString();
        message.Should().Be("Ok");
    }
}