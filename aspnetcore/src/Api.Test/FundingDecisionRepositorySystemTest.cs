using Api.DataAccess.Repositories;
using Api.Models.Entities;
using Api.Models.FundingDecision;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
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
                .Where(x => x.NameFi == "Tekoälyteknologioita vuorovaikutusten ennustamiseen biolääketieteessä")
                .ToList();

            // Assert
            var e = entities.Where(x => x.OrganizationConsortia.Any()).FirstOrDefault();
            e.Should().NotBeNull();

        }

    }
}
