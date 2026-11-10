using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Organization;
using CSC.PublicApi.Service.Models.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using InfrastructureService = CSC.PublicApi.Service.Models.Infrastructure.Service;

namespace Repository.Tests;

[Collection(SqlServerRepositoryCollection.Name)]
[Trait("Category", "SqlServerIntegration")]
public sealed class InfrastructureServiceSqlServerTest(SqlServerRepositoryFixture fixture)
{
    [Fact]
    public async Task PerformInMemoryOperation_EnrichesInfrastructureFromSqlAndOrganizationCache()
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO dim_infrastructure
                (id, local_identifier, acronym, finland_roadmap, responsible_organization_id,
                 dim_start_date, dim_end_date, dim_registered_data_source_id, source_id, source_description)
            VALUES (900, N'local-infra', 'SYN', 1, 101, -1, -1, -1, N'infra-900', N'synthetic');

            INSERT INTO dim_pid
                (id, pid_content, pid_type, dim_organization_id, dim_known_person_id, dim_infrastructure_id)
            VALUES
                (980, N'urn:nbn:fi:ttv-infrastructure-900', N'URN', -1, -1, 900),
                (981, N'10.1234/infra-900', N'DOI', -1, -1, 900);

            INSERT INTO fact_contribution
                (dim_publication_id, dim_infrastructure_id, dim_organization_id, contribution_type)
            VALUES
                (-1, 900, 102, N'organization_participate_infrastructure'),
                (-1, 900, 102, N'organization_participate_infrastructure'),
                (-1, 900, 103, N'other_contribution');

            INSERT INTO dim_descriptive_item
                (id, dim_start_date, descriptive_item, descriptive_item_type, descriptive_item_language,
                 source_id, dim_research_project_id, dim_registered_data_source_id, dim_publication_id,
                 dim_research_dataset_id, dim_infrastructure_id, dim_service_id, dim_research_data_catalog_id)
            VALUES
                (990, -1, N'Synthetic infrastructure name', N'name', 'en', N'descriptive-990',
                 -1, -1, -1, -1, 900, -1, -1);

            INSERT INTO dim_referencedata
                (id, code_value, code_scheme, name_en, name_fi, name_sv, source_id, source_description)
            VALUES (991, N'SYN-ESFRI', N'ESFRI-Domain', N'Synthetic domain', NULL, NULL, N'ref-991', N'synthetic');

            INSERT INTO fact_referencedata (dim_referencedata_id, dim_infrastructure_id)
            VALUES (991, 900);
            """);

        using var cache = new MemoryCache(new MemoryCacheOptions());
        cache.Set(MemoryCacheKeys.OrganizationById(101), Organization(101, "Responsible organization"));
        cache.Set(MemoryCacheKeys.OrganizationById(102), Organization(102, "Participating organization"));
        var repository = new InfrastructureServiceIndexRepository(context, null!, cache);
        var service = new InfrastructureService
        {
            DimInfrastructureId = 900,
            Pids = new List<PersistentIdentifier>
            {
                new() { Type = "URN", Content = "urn:nbn:fi:ttv-service-900" }
            }
        };

        repository.PerformInMemoryOperation(service);

        var infrastructure = Assert.IsType<ServiceInfrastructure>(service.IsPartOfInfrastructure);
        Assert.Equal("urn:nbn:fi:ttv-infrastructure-900", infrastructure.InfraIdentifier!.KeyIdentifierURN);
        Assert.Equal("https://urn.fi/urn:nbn:fi:ttv-infrastructure-900", infrastructure.InfraIdentifier.KeyIdentifierURNLink);
        Assert.Equal("doi", Assert.Single(infrastructure.InfraIdentifier.OtherPid!).PidType);
        Assert.Equal("10.1234/infra-900", infrastructure.InfraIdentifier.OtherPid![0].PidContent);
        Assert.Null(infrastructure.Pids);
        Assert.Equal(
            "Responsible organization",
            infrastructure.InfraResponsibleOrganization!.OrganizationName!.En);
        Assert.Null(infrastructure.InfraResponsibleOrganization.DimOrganizationId);
        var participant = Assert.Single(infrastructure.InfraParticipatingOrganizations!);
        Assert.Equal("Participating organization", participant.OrganizationName!.En);
        Assert.Null(participant.DimOrganizationId);
        Assert.Equal("1234567-8", Assert.Single(participant.OrganizationIdentifier!).PidContent);
        Assert.Equal("Synthetic infrastructure name", Assert.Single(infrastructure.InfraName!).TextContent);
        Assert.Equal("SYN-ESFRI", Assert.Single(infrastructure.ESFRICodes!).CodeValue);
        Assert.Equal(
            "https://research.fi/en/results/infrastructure/ttv-infrastructure-900?service=ttv-service-900",
            service.ServiceResearchfiURL!.En);
        Assert.Equal(
            "https://research.fi/en/results/infrastructure/ttv-infrastructure-900",
            infrastructure.ResearchfiURL!.En);

        await transaction.RollbackAsync();
    }

    private static Organization Organization(int id, string name) => new()
    {
        Id = id,
        NameEn = name,
        Pids = new List<PersistentIdentifier>
        {
            new() { Type = "BUSINESSID", Content = "1234567-8" }
        }
    };
}
