using Api.DataAccess.Repositories;
using Api.Models.FundingCall;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
    public class FundingCallRepositorySystemTest: IClassFixture<TestWebApplicationFactory<Program>>
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
            var entities = await _repository.GetAllAsync()
                .Where(x => x.NameFi == "some name")
                .ToListAsync();

            // Assert
            entities.Should().NotBeEmpty();

        }
    }
}
