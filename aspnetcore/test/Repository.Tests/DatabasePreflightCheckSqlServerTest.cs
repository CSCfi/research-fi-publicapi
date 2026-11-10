using CSC.PublicApi.Indexer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Repository.Tests;

[Collection(SqlServerRepositoryCollection.Name)]
[Trait("Category", "SqlServerIntegration")]
public sealed class DatabasePreflightCheckSqlServerTest(SqlServerRepositoryFixture fixture)
{
    [Fact]
    public async Task IsGood_LogsCountsAndStatusForSeededTables()
    {
        await using var context = fixture.CreateContext();
        var logger = new CapturingLogger();
        var preflight = new DatabasePreflightCheck(context, logger);

        var isGood = preflight.IsGood();

        Assert.True(isGood);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Information && entry.Message.Contains("dim_publication count = 5"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Information &&
            entry.Message.Contains("Number of distinct dim_publication references in fact_contribution = 4"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Information && entry.Message.Contains("status OK"));
        Assert.DoesNotContain(logger.Entries, entry => entry.Level == LogLevel.Error);
    }

    [Fact]
    public async Task IsGood_LogsFailureWhenDistinctPublicationReferencesAreBelowThreshold()
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.Database.ExecuteSqlRawAsync(
            "DELETE FROM fact_contribution WHERE dim_publication_id = 4");
        var logger = new CapturingLogger();
        var preflight = new DatabasePreflightCheck(context, logger);

        var isGood = preflight.IsGood();

        Assert.False(isGood);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error &&
            entry.Message.Contains("Possibly too few of dim_publication references in fact_contribution"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("indexing aborted"));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task IsGood_ExcludesSentinelRowsAndLogsEmptyRequiredTables()
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.Database.ExecuteSqlRawAsync(
            "DELETE FROM dim_publication WHERE id > 0; DELETE FROM dim_research_dataset WHERE id > 0;");
        var logger = new CapturingLogger();
        var preflight = new DatabasePreflightCheck(context, logger);

        var isGood = preflight.IsGood();

        Assert.False(isGood);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Information && entry.Message.Contains("dim_publication count = 0"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("Table dim_publication is empty"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("Table dim_research_dataset is empty"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Information &&
            entry.Message.Contains("Number of distinct dim_publication references in fact_contribution = 4"));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("indexing aborted"));
        await transaction.RollbackAsync();
    }

    private sealed class CapturingLogger : ILogger<DatabasePreflightCheck>
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, formatter(state, exception)));
        }
    }
}
