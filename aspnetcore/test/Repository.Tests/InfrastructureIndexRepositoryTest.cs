using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using InfrastructureOrganization = CSC.PublicApi.Service.Models.Organization.Organization;

namespace Repository.Tests;

public class InfrastructureIndexRepositoryTest
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PerformInMemoryOperation_ResolvesCachedOrganizationsAndIdentifiers(bool useBulkOperation)
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        cache.Set(MemoryCacheKeys.OrganizationById(10), new InfrastructureOrganization
        {
            Id = 10,
            Pids = new List<PersistentIdentifier>
            {
                new() { Type = "BUSINESSID", Content = "1234567-8" }
            }
        });
        cache.Set(MemoryCacheKeys.OrganizationById(20), new InfrastructureOrganization
        {
            Id = 20,
            Pids = new List<PersistentIdentifier>()
        });
        var repository = new InfrastructureIndexRepository(null!, null!, cache);
        var infrastructure = new Infrastructure
        {
            Pids = new List<PersistentIdentifier>
            {
                new() { Type = "URN", Content = "urn:nbn:fi:ttv-infrastructure" }
            },
            InfraResponsibleOrganization = new ResearchOrganization
            {
                DimOrganizationId = 10
            },
            InfraParticipatingOrganizations = new List<ResearchOrganization>
            {
                new() { DimOrganizationId = 20 },
                new() { DimOrganizationId = 30 }
            },
            InfraRelations = new List<InfrastructureNetwork>(),
            InternationalInfraRelationsHelper = new List<InfrastructureNetwork>()
        };

        if (useBulkOperation)
            repository.PerformInMemoryOperations(new List<object> { infrastructure, new object() });
        else
            repository.PerformInMemoryOperation(infrastructure);

        Assert.Null(infrastructure.Pids);
        Assert.Null(infrastructure.InfraResponsibleOrganization.DimOrganizationId);
        Assert.Equal("1234567-8", Assert.Single(infrastructure.InfraResponsibleOrganization.OrganizationIdentifier!).PidContent);
        Assert.Null(infrastructure.InfraParticipatingOrganizations[0].DimOrganizationId);
        Assert.Empty(infrastructure.InfraParticipatingOrganizations[0].OrganizationIdentifier!);
        Assert.Null(infrastructure.InfraParticipatingOrganizations[1].DimOrganizationId);
        Assert.Null(infrastructure.InfraParticipatingOrganizations[1].OrganizationIdentifier);
        Assert.Equal(
            "https://research.fi/en/results/infrastructure/ttv-infrastructure",
            infrastructure.InfraResearchfiURL?.En);
        Assert.Null(infrastructure.InfraRelations);
        Assert.Empty(infrastructure.InternationalInfraRelationsHelper!);
    }

    [Fact]
    public void PerformInMemoryOperation_WithoutInfrastructureUrnDoesNotCreateResearchfiLink()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var repository = new InfrastructureIndexRepository(null!, null!, cache);
        var infrastructure = new Infrastructure
        {
            Pids = new List<PersistentIdentifier>
            {
                new() { Type = "DOI", Content = "10.1234/infra" }
            },
            InfraRelations = new List<InfrastructureNetwork>(),
            InternationalInfraRelationsHelper = new List<InfrastructureNetwork>()
        };

        repository.PerformInMemoryOperation(infrastructure);

        Assert.Null(infrastructure.InfraResearchfiURL);
        Assert.Equal("doi", Assert.Single(infrastructure.InfraIdentifier!.OtherPid!).PidType);
        Assert.Null(infrastructure.InfraRelations);
    }
}
