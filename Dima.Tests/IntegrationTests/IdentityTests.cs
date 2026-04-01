using System.Net;
using System.Net.Http.Json;
using Dima.Core.Requests.Account;
using FluentAssertions;

namespace Dima.Tests.IntegrationTests;

public class IdentityTests(DimaWebApplicationFactory factory) : IClassFixture<DimaWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_Register_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = $"test_{Guid.NewGuid()}@dima.com",
            Password = "Password123!",
            GenerateDemoData = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/v1/identity/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_Register_ShouldReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "invalid-email",
            Password = "123",
            GenerateDemoData = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/v1/identity/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}