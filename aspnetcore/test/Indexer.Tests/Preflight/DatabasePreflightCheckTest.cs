
using FluentAssertions;
using Xunit;

namespace CSC.PublicApi.Indexer.Tests.Preflight;

public class DatabasePreflightCheckTest
{
    public DatabasePreflightCheckTest()
    {
    }

    [Fact]
    public void FactContributionNumberOfDistinctReferencesToDimPublicationIsGood_01()
    {
        // Arrange
        DatabasePreflightCheck databasePreflightCheck = new DatabasePreflightCheck();
        int dimPublicationCount = 10;
        int factContributionDistinctReferencesToDimPublicationCount = 7;

        // Act
        bool actualResult = databasePreflightCheck.FactContributionNumberOfDistinctReferencesToDimPublicationIsGood(
            dimPublicationCount,
            factContributionDistinctReferencesToDimPublicationCount);

        // Assert
        Assert.False(actualResult);
    }

    [Fact]
    public void FactContributionNumberOfDistinctReferencesToDimPublicationIsGood_02()
    {
        // Arrange
        DatabasePreflightCheck databasePreflightCheck = new DatabasePreflightCheck();
        int dimPublicationCount = 10;
        int factContributionDistinctReferencesToDimPublicationCount = 9;

        // Act
        bool actualResult = databasePreflightCheck.FactContributionNumberOfDistinctReferencesToDimPublicationIsGood(
            dimPublicationCount,
            factContributionDistinctReferencesToDimPublicationCount);

        // Assert
        Assert.True(actualResult);
    }
}