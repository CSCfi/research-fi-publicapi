using System;
using System.Linq;
using System.Net.Http;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FundingCall = ResearchFi.FundingCall.FundingCall;

namespace CSC.PublicApi.Interface.Tests;

public class FundingCallPostSystemTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public FundingCallPostSystemTest(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact(Skip = "Awaiting API-owner decision; the current service does not persist funding calls.")]
    public async System.Threading.Tasks.Task PostFundingCall_ShouldPersistToDatabase()
    {
        var fundingCallToAdd = new FundingCall
        {
            NameFi = $"test funding call {Guid.NewGuid()}"
        };
        using var content = System.Net.Http.Json.JsonContent.Create(fundingCallToAdd);
        using var response = await _client.PostAsync("v1/funding-calls", content);

        response.EnsureSuccessStatusCode();
        using var scope = _factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        var addedFundingCall = dbContext.Set<DimCallProgramme>()
            .SingleOrDefault(entity => entity.NameFi == fundingCallToAdd.NameFi);

        Xunit.Assert.NotNull(addedFundingCall);
    }
}
