using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.FundingDecision;
using CSC.PublicApi.Service.Models.ResearchDataset;
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
    public void GetIndexNameForType_FundingDecision_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.FundingDecision.FundingDecision"] = "some index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(FundingDecision));

        // Assert
        actual.Should().Be("some index name");

    }

    [Fact]
    public void GetIndexNameForType_FundingCall_ShouldReturnIndexName()
    {
        // Arrange
        _settings["CSC.PublicApi.Service.Models.FundingCall.FundingCall"] = "some index name";

        // Act
        var actual = _settings.GetIndexNameForType(typeof(FundingCall));

        // Assert
        actual.Should().Be("some index name");

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
}