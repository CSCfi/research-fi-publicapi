﻿using System.Net.Http;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class SwaggerSystemTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SwaggerSystemTest(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact(Skip = "Integration test")]
    public async void SwaggerJson_ShouldLoad()
    {
        // Arrange
        var apiUrl = $"swagger/v1/swagger.json";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}