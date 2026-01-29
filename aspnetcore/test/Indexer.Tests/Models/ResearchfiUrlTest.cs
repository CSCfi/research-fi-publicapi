
using CSC.PublicApi.Service.Models;
using FluentAssertions;
using Xunit;

namespace CSC.PublicApi.Indexer.Tests.Models;

public class ResearchfiUrlTest
{
    public ResearchfiUrlTest()
    {
    }

    [Fact]
    public void ResearchfiUrl_Creation_WithFundingCallResourceType_ShouldSetUrlsCorrectly()
    {
        // Arrange
        string resourceType = "funding-call";
        string id = "12345";

        // Act
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(resourceType, id);

        // Assert
        researchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/funding-call/12345");
        researchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/funding-call/12345");
        researchfiUrl.En.Should().Be("https://research.fi/en/results/funding-call/12345");
    }

    [Fact]
    public void ResearchfiUrl_Creation_WithFundingDecisionResourceType_ShouldSetUrlsCorrectly()
    {
        // Arrange
        string resourceType = "funding-decision";
        string id = "67890";

        // Act
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(resourceType, id);

        // Assert
        researchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/funding/67890");
        researchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/funding/67890");
        researchfiUrl.En.Should().Be("https://research.fi/en/results/funding/67890");
    }

    [Fact]
    public void ResearchfiUrl_Creation_WithPublicationResourceType_ShouldSetUrlsCorrectly()
    {
        // Arrange
        string resourceType = "publication";
        string id = "pub-001";

        // Act
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(resourceType, id);

        // Assert
        researchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/publication/pub-001");
        researchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/publication/pub-001");
        researchfiUrl.En.Should().Be("https://research.fi/en/results/publication/pub-001");
    }

    [Fact]
    public void ResearchfiUrl_Creation_WithResearchDatasetResourceType_ShouldSetUrlsCorrectly()
    {
        // Arrange
        string resourceType = "research-dataset";
        string id = "dataset-123";

        // Act
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(resourceType, id);

        // Assert
        researchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/dataset/dataset-123");
        researchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/dataset/dataset-123");
        researchfiUrl.En.Should().Be("https://research.fi/en/results/dataset/dataset-123");
    }

    [Fact]
    public void ResearchfiUrl_Creation_WithInfrastructureResourceType_ShouldSetUrlsCorrectly()
    {
        // Arrange
        string resourceType = "infrastructure";
        string id = "urn:nbn:fi:ttv-202509000687197";
        // Act
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(resourceType, id);
        // Assert
        researchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/infrastructure/ttv-202509000687197");
        researchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/infrastructure/ttv-202509000687197");
        researchfiUrl.En.Should().Be("https://research.fi/en/results/infrastructure/ttv-202509000687197");
    }

        [Fact]
    public void ResearchfiUrl_Creation_WithInfrastructureServiceResourceType_ShouldSetUrlsCorrectly()
    {
        // Arrange
        string resourceType = "infrastructure-service";
        string infrastructureURN = "urn:nbn:fi:ttv-11223344556677";
        string infrastructureServiceURN = "urn:nbn:fi:ttv-77665544332211";
        // Act
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(resourceType, infrastructureURN, infrastructureServiceURN);
        // Assert
        researchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/infrastructure/ttv-11223344556677?service=ttv-77665544332211");
        researchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/infrastructure/ttv-11223344556677?service=ttv-77665544332211");
        researchfiUrl.En.Should().Be("https://research.fi/en/results/infrastructure/ttv-11223344556677?service=ttv-77665544332211");
    }
}