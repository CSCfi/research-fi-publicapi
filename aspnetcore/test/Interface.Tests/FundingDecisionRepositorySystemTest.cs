using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models.FundingDecision;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class FundingDecisionRepositorySystemTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly IIndexRepository<FundingDecision> _repository;

    public FundingDecisionRepositorySystemTest(TestWebApplicationFactory<Program> factory)
    {
        var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        _repository = scope.ServiceProvider.GetRequiredService<IIndexRepository<FundingDecision>>();
    }

    [Fact(Skip = "Currently used only for manual debugging.")]
    public async Task Repo_ShouldReturn_Something()
    {
        var entities = _repository.GetAll()
            .Where(x => x.NameFi == "Mitokondriometabolia hermoston terveyden ja sairauden säätelijänä")
            .ToList();

        // Assert
        entities.Should().NotBeEmpty();
        var e = entities.First();
        e.FundingGroupPerson.Should().NotBeNullOrEmpty();
        var p = e.FundingGroupPerson.First();
        p.OrcId.Should().NotBeNullOrEmpty();
        p.RoleInFundingGroup.Should().NotBeNullOrEmpty();

    }

    [Fact(Skip = "Currently used only for manual debugging.")]
    public async Task Repo_ShouldReturn_Something2()
    {
        var entities = _repository.GetAll()
            .Where(x => x.NameFi == "Tekoälyteknologioita vuorovaikutusten ennustamiseen biolääketieteessä")
            .ToList();

        // Assert
        // 4 entities found
        entities.Should().NotBeEmpty();
        var e = entities.First();
        e.FundingGroupPerson.Should().NotBeNullOrEmpty();
        e.OrganizationConsortia.Should().NotBeNullOrEmpty();
        var p = e.FundingGroupPerson.First();
        p.OrcId.Should().NotBeNullOrEmpty();
        p.RoleInFundingGroup.Should().NotBeNullOrEmpty();


    }

    [Fact(Skip = "Currently used only for manual debugging.")]
    public async Task Repo_ShouldReturn_Something3()
    {
        var entities = _repository.GetAll()
            .Where(x => x.Acronym.Contains("USSEE") || x.Acronym == "PandeVITA")
            //.Where(x => x.NameFi.Contains("screenshotin "))
            .Cast<object>()
            .ToList();
        entities = _repository.PerformInMemoryOperations(entities);

        // Assert
        entities.Should().HaveCount(2);
        var entity = entities.First();
        var x = JsonSerializer.Serialize(entities.First());

    }

}