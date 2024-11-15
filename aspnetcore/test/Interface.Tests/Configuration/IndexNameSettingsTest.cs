using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.FundingDecision;
using CSC.PublicApi.Service.Models.ResearchDataset;
using CSC.PublicApi.Service.Models.Publication;
using CSC.PublicApi.Service.Models.Infrastructure;
using CSC.PublicApi.Service.Models.Organization;
using FluentAssertions;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Configuration;

public class IndexNameSettingsTest
{
    private IndexNameSettings _settings;

    public IndexNameSettingsTest()
    {
        _settings = new IndexNameSettings();
    }

    [Fact]
    public void GetIndexNameForType_Publication_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.Publication.Publication"] = "publication index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(Publication));

        // Assert
        actual.Should().Be("publication index name");
    }

    [Fact]
    public void GetIndexNameForType_FundingDecision_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.FundingDecision.FundingDecision"] = "funding decision index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(FundingDecision));

        // Assert
        actual.Should().Be("funding decision index name");
    }

    [Fact]
    public void GetIndexNameForType_FundingCall_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.FundingCall.FundingCall"] = "funding call index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(FundingCall));

        // Assert
        actual.Should().Be("funding call index name");
    }

    [Fact]
    public void GetIndexNameForType_ResearchDataset_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.ResearchDataset.ResearchDataset"] = "some index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(ResearchDataset));

        // Assert
        actual.Should().Be("some index name");
    }

    [Fact]
    public void GetIndexNameForType_Infrastructure_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.Infrastructure.Infrastructure"] = "infrastructure index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(Infrastructure));

        // Assert
        actual.Should().Be("infrastructure index name");
    }

    [Fact]
    public void GetIndexNameForType_Organization_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.Organization.Organization"] = "organization index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(Service.Models.Organization.Organization));

        // Assert
        actual.Should().Be("organization index name");
    }
}