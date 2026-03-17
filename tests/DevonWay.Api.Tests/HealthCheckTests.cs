using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace DevonWay.Api.Tests;

public class HealthCheckTests
{
    [Fact]
    public async Task Health_Endpoint_Returns_OK()
    {
        // Arrange
        await using var app = new WebApplicationFactory<Program>();
        using var client = app.CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Incidents_Endpoint_Returns_List()
    {
        await using var app = new WebApplicationFactory<Program>();
        using var client = app.CreateClient();

        var response = await client.GetAsync("/incidents");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Slip hazard", content);
    }
}
