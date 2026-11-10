
using Xunit;

namespace CSC.PublicApi.Indexer.Tests.Preflight;

public class DatabasePreflightCheckTest
{
    public DatabasePreflightCheckTest()
    {
    }

    [Theory]
    [InlineData(10, 7, false)]
    [InlineData(10, 8, true)]
    [InlineData(100, 79, false)]
    [InlineData(100, 80, true)]
    [InlineData(100, 81, true)]
    [InlineData(0, 0, true)]
    public void FactContributionNumberOfDistinctReferencesToDimPublicationIsGood_UsesEightyPercentThreshold(
        int publicationCount,
        int distinctReferenceCount,
        bool expected)
    {
        var preflight = new DatabasePreflightCheck();

        var actual = preflight.FactContributionNumberOfDistinctReferencesToDimPublicationIsGood(
            publicationCount,
            distinctReferenceCount);

        Assert.Equal(expected, actual);
    }
}