using CSC.PublicApi.DatabaseContext;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Xunit;

namespace Repository.Tests;

[CollectionDefinition(Name)]
public sealed class SqlServerRepositoryCollection : ICollectionFixture<SqlServerRepositoryFixture>
{
    public const string Name = "SQL Server repository integration";
}

public sealed class SqlServerRepositoryFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder(
            "mcr.microsoft.com/mssql/server:2022-CU27-ubuntu-22.04")
        .WithDatabase($"researchfi_test_{Guid.NewGuid():N}")
        .WithPassword($"Test!{Guid.NewGuid():N}")
        .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
        .Build();

    public ApiDbContext CreateContext()
    {
        return new ApiDbContext(new DbContextOptionsBuilder<ApiDbContext>()
            .UseSqlServer(_container.GetConnectionString())
            .Options);
    }

    public async Task InitializeAsync()
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        await _container.StartAsync(timeout.Token);

        await using var context = CreateContext();
        foreach (var script in new[]
                 {
                     "FundingCallSchema.sql",
                     "FundingDecisionSchema.sql",
                     "PreflightAndInfrastructureSchema.sql"
                 })
        {
            await using var schema = typeof(SqlServerRepositoryFixture).Assembly.GetManifestResourceStream(
                $"Repository.Tests.SqlServer.{script}")
                ?? throw new InvalidOperationException($"The test schema resource '{script}' is missing.");
            using var reader = new StreamReader(schema);
            var sql = await reader.ReadToEndAsync(timeout.Token);
            await context.Database.ExecuteSqlRawAsync(sql, timeout.Token);
        }
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
