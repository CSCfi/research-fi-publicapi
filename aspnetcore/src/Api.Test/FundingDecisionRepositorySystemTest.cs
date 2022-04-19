using Api.DataAccess.Repositories;
using Api.Models.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
    public class FundingDecisionRepositorySystemTest: IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly IGenericRepository<DimFundingDecision> _repository;

        public FundingDecisionRepositorySystemTest(TestWebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IFundingDecisionRepository>();
        }

        [Fact]
        public async Task Repo_ShouldReturn_Something()
        {
            var entities = _repository.GetAll()
                //.Where(x => x.NameEn != null && x.NameEn.Contains("raha", StringComparison.CurrentCultureIgnoreCase))
                //.Where(x => x.DimWebLinks != null && x.DimWebLinks.Any(l => l.LinkType == "ApplicationURL"))
                //.Where(x => x.DimDateIdDueNavigation != null)
                //.Where(x => x.DimDateIdOpen == -1)
                .ToList();

            // Assert
            entities.SingleOrDefault(e => e.Id == -1).Should().BeNull();
            entities.Should().NotContain(e => e.DimTypeOfFunding == null);
            entities.Should().NotContain(e => e.DimTypeOfFunding != null && e.DimTypeOfFunding.TypeId == "62");
            entities.Should().NotContain(e => e.DimTypeOfFunding != null && e.DimTypeOfFunding.TypeId == "66");
            entities.Should().NotContain(e => e.DimTypeOfFunding != null && e.DimTypeOfFunding.TypeId == "69");

            entities
                .Should().HaveCount(36699);

        }

    }
}
