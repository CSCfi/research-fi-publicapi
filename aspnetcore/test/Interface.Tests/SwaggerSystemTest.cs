using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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

    [Fact]
    public async Task SwaggerJson_V1_ShouldLoadAndContainCorrectData()
    {
        var expectedPaths_V1 = new[]
        {
            "/VirtaJulkaisutietopalvelu/Latausraportit/duplikaatit",
            "/VirtaJulkaisutietopalvelu/Latausraportit/tilanne",
            "/VirtaJulkaisutietopalvelu/Latausraportit/virheet",
            "/VirtaJulkaisutietopalvelu/Yhteisjulkaisut/ristiriitaiset",
            "/v1/funders/{grantedFundingId}/publications/{organizationId}",
            "/v1/funders/{grantedFundingId}/publications/{organizationId}/{publicationIdentifier}",
            "/v1/funding-calls",
            "/v1/funding-calls-export",
            "/v1/funding-decisions",
            "/v1/funding-decisions-export",
            "/v1/infrastructures",
            "/v1/infrastructures-export",
            "/v1/infrastructures-export/services",
            "/v1/infrastructures/services",
            "/v1/publications",
            "/v1/publications-export",
            "/v1/publications/{publicationId}",
            "/v1/research-datasets",
            "/v1/research-datasets-export"
        };

        // Arrange
        var apiUrl = $"swagger/v1/swagger.json";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        response.EnsureSuccessStatusCode();

        // Check that the response contains valid JSON
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        Assert.NotNull(jsonDocument);

        // Check that the JSON contains expected properties
        Assert.True(jsonDocument.RootElement.TryGetProperty("openapi", out _));
        Assert.True(jsonDocument.RootElement.TryGetProperty("info", out _));

        // Check that the version in the info section is correct
        var infoElement = jsonDocument.RootElement.GetProperty("info");
        Assert.True(infoElement.TryGetProperty("version", out var versionElement));
        Assert.Equal("1.0", versionElement.GetString());

        // Check that the paths property exists
        Assert.True(jsonDocument.RootElement.TryGetProperty("paths", out var pathsElement));

        // Check that all expected endpoints exist in paths
        var actualPaths = pathsElement.EnumerateObject().Select(p => p.Name).ToHashSet(StringComparer.Ordinal);

        foreach (var expectedPath in expectedPaths_V1)
        {
            Assert.True(
                actualPaths.Contains(expectedPath),
                $"Swagger is missing expected path '{expectedPath}'.");
        }
    }

    [Fact]
    public async Task SwaggerJson_V2_ShouldLoadAndContainCorrectData()
    {
        var expectedPaths_V2 = new[]
        {
            "/v2/research-datasets"
        };

        // Arrange
        var apiUrl = $"swagger/v2/swagger.json";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        response.EnsureSuccessStatusCode();

        // Check that the response contains valid JSON
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        Assert.NotNull(jsonDocument);

        // Check that the JSON contains expected properties
        Assert.True(jsonDocument.RootElement.TryGetProperty("openapi", out _));
        Assert.True(jsonDocument.RootElement.TryGetProperty("info", out _));

        // Check that the version in the info section is correct
        var infoElement = jsonDocument.RootElement.GetProperty("info");
        Assert.True(infoElement.TryGetProperty("version", out var versionElement));
        Assert.Equal("2.0", versionElement.GetString());

        // Check that the paths property exists
        Assert.True(jsonDocument.RootElement.TryGetProperty("paths", out var pathsElement));

        // Check that all expected endpoints exist in paths
        var actualPaths = pathsElement.EnumerateObject().Select(p => p.Name).ToHashSet(StringComparer.Ordinal);

        foreach (var expectedPath in expectedPaths_V2)
        {
            Assert.True(
                actualPaths.Contains(expectedPath),
                $"Swagger is missing expected path '{expectedPath}'.");
        }
    }
}