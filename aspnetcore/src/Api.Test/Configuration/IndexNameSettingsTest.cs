using Api.Configuration;
using Api.Models.FundingDecision;
using FluentAssertions;
using Xunit;

namespace Api.Test.Configuration
{
    public class IndexNameSettingsTest
    {
        private IndexNameSettings _settings;

        public IndexNameSettingsTest()
        {
            _settings = new IndexNameSettings();
        }

        [Fact]
        public void GetIndexNameForType_ShouldReturnIndexName()
        {
            // Arrange
            _settings["Api.Models.FundingDecision.FundingDecision"] = "some index name";

            // Act
            var actual = _settings.GetIndexNameForType(typeof(FundingDecision));

            // Assert
            actual.Should().Be("some index name");

        }
    }
}
