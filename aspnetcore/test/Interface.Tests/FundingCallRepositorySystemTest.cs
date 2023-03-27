using System.Linq;
using System.Threading.Tasks;
using ResearchFi.FundingCall;
using CSC.PublicApi.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class FundingCallRepositorySystemTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly IIndexRepository<FundingCall> _repository;

    public FundingCallRepositorySystemTest(TestWebApplicationFactory<Program> factory)
    {
        var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        _repository = scope.ServiceProvider.GetRequiredService<IIndexRepository<FundingCall>>();
    }

    [Fact(Skip = "Currently used only for manual debugging.")]
    public async Task Repo_ShouldReturn_Something()
    {
        var entities = _repository.GetAll()
            .Where(x => x.NameFi == "some name")
            .ToList();

        // Assert
        entities.Should().NotBeEmpty();

    }
}