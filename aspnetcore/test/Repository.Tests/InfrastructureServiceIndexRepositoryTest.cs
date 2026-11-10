using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using InfrastructureService = CSC.PublicApi.Service.Models.Infrastructure.Service;

namespace Repository.Tests;

public class InfrastructureServiceIndexRepositoryTest
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PerformInMemoryOperation_TransformsServiceIdentifiersAndBuildsResearchfiLinks(bool useBulkOperation)
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var repository = new InfrastructureServiceIndexRepository(null!, null!, cache);
        var service = new InfrastructureService
        {
            LocalIdentifier = "local-service-id",
            DimInfrastructureId = 0,
            Pids = new List<PersistentIdentifier>
            {
                new() { Type = "URN", Content = "urn:nbn:fi:ttv-service" },
                new() { Type = "DOI", Content = "10.1234/service" }
            },
            IsPartOfInfrastructure = new ServiceInfrastructure
            {
                InfraIdentifier = new Identifier
                {
                    KeyIdentifierURN = "urn:nbn:fi:ttv-infrastructure"
                }
            },
            ServiceName = new List<DescriptiveText>()
        };

        if (useBulkOperation)
            repository.PerformInMemoryOperations(new List<object> { service, new object() });
        else
            repository.PerformInMemoryOperation(service);

        Assert.Null(service.Pids);
        Assert.NotNull(service.ServiceIdentifier);
        Assert.Equal("local-service-id", service.ServiceIdentifier.LocalIdentifier);
        Assert.Equal("urn:nbn:fi:ttv-service", service.ServiceIdentifier.KeyIdentifierURN);
        Assert.Equal("https://urn.fi/urn:nbn:fi:ttv-service", service.ServiceIdentifier.KeyIdentifierURNLink);
        Assert.Equal("doi", Assert.Single(service.ServiceIdentifier.OtherPid).PidType);
        Assert.Equal("10.1234/service", service.ServiceIdentifier.OtherPid[0].PidContent);
        Assert.Null(service.ServiceName);
        Assert.Equal(
            "https://research.fi/en/results/infrastructure/ttv-infrastructure?service=ttv-service",
            service.ServiceResearchfiURL?.En);
        Assert.Equal(
            "https://research.fi/en/results/infrastructure/ttv-infrastructure",
            service.IsPartOfInfrastructure.ResearchfiURL?.En);
    }

    [Fact]
    public void PerformInMemoryOperation_WithoutPidsDoesNotCreateAnIdentifierOrServiceLink()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var repository = new InfrastructureServiceIndexRepository(null!, null!, cache);
        var service = new InfrastructureService
        {
            DimInfrastructureId = 0,
            Pids = new List<PersistentIdentifier>()
        };

        repository.PerformInMemoryOperation(service);

        Assert.Null(service.Pids);
        Assert.Null(service.ServiceIdentifier);
        Assert.Null(service.ServiceResearchfiURL);
    }
}
