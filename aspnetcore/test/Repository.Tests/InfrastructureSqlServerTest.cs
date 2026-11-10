using AutoMapper;
using CSC.PublicApi.Repositories;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using Infrastructure = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;
using InfrastructureOrganization = CSC.PublicApi.Service.Models.Organization.Organization;

namespace Repository.Tests;

[Collection(SqlServerRepositoryCollection.Name)]
[Trait("Category", "SqlServerIntegration")]
public sealed class InfrastructureSqlServerTest(SqlServerRepositoryFixture fixture)
{
    private readonly IMapper _mapper = new MapperConfiguration(
        configuration => configuration.AddProfile<InfrastructureProfile>()).CreateMapper();

    [Fact]
    public async Task GetAll_ProjectsInfrastructureRowsAndPostProcessesIdentifiersAndOrganizations()
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO dim_infrastructure
                (id, local_identifier, acronym, finland_roadmap, responsible_organization_id,
                 dim_start_date, dim_end_date, dim_registered_data_source_id, source_id, source_description)
            VALUES
                (910, N'local-infra-910', 'SQL', 1, 101, 1, 2, -1, N'infra-910', N'synthetic'),
                (911, N'old-infra-911', 'OLD', 0, -1, -1, -1, -1, N'infra-911', N'infra_old'),
                (-1, N'sentinel-infra', NULL, 0, -1, -1, -1, -1, N'infra-sentinel', N'synthetic');

            INSERT INTO dim_pid
                (id, pid_content, pid_type, dim_organization_id, dim_known_person_id, dim_infrastructure_id)
            VALUES (982, N'urn:nbn:fi:ttv-infrastructure-910', N'URN', -1, -1, 910);

            INSERT INTO dim_descriptive_item
                (id, dim_start_date, descriptive_item, descriptive_item_type, descriptive_item_language,
                 source_id, dim_research_project_id, dim_registered_data_source_id, dim_publication_id,
                 dim_research_dataset_id, dim_infrastructure_id, dim_service_id, dim_research_data_catalog_id)
            VALUES (992, -1, N'Projected infrastructure', N'name', 'en', N'name-910',
                -1, -1, -1, -1, 910, -1, -1);

            INSERT INTO fact_contribution
                (dim_publication_id, dim_infrastructure_id, dim_organization_id, contribution_type)
            VALUES (-1, 910, 102, N'organization_participate_infrastructure');
            """);

        using var cache = new MemoryCache(new MemoryCacheOptions());
        cache.Set(MemoryCacheKeys.OrganizationById(101), Organization(101, "Responsible"));
        cache.Set(MemoryCacheKeys.OrganizationById(102), Organization(
            102,
            "Participant",
            new PersistentIdentifier { Type = "BUSINESSID", Content = "1234567-8" }));
        IIndexRepository<Infrastructure> repository = new InfrastructureIndexRepository(context, _mapper, cache);

        var entities = await repository.GetAll().ToListAsync();

        var infrastructure = Assert.Single(entities);
        Assert.Equal(910L, infrastructure.ExportSortId);
        Assert.Equal("local-infra-910", infrastructure.LocalIdentifier);
        Assert.Equal("SQL", infrastructure.InfraAcronym);
        Assert.Equal((2026, 1, 15),
            (infrastructure.InfraStartsOn!.Year, infrastructure.InfraStartsOn.Month, infrastructure.InfraStartsOn.Day));
        Assert.Equal((2026, 2, 28),
            (infrastructure.InfraEndsOn!.Year, infrastructure.InfraEndsOn.Month, infrastructure.InfraEndsOn.Day));
        Assert.Equal("Projected infrastructure", Assert.Single(infrastructure.InfraName!).TextContent);
        Assert.Equal("Synthetic foundation", infrastructure.InfraResponsibleOrganization!.OrganizationName!.En);
        Assert.Equal(101, infrastructure.InfraResponsibleOrganization.DimOrganizationId);
        Assert.Equal("Second synthetic foundation",
            Assert.Single(infrastructure.InfraParticipatingOrganizations!).OrganizationName!.En);
        Assert.Equal(102, Assert.Single(infrastructure.InfraParticipatingOrganizations!).DimOrganizationId);

        repository.PerformInMemoryOperation(infrastructure);

        Assert.Equal("urn:nbn:fi:ttv-infrastructure-910", infrastructure.InfraIdentifier!.KeyIdentifierURN);
        Assert.Null(infrastructure.InfraResponsibleOrganization.DimOrganizationId);
        Assert.Empty(infrastructure.InfraResponsibleOrganization.OrganizationIdentifier!);
        var participant = Assert.Single(infrastructure.InfraParticipatingOrganizations!);
        Assert.Null(participant.DimOrganizationId);
        Assert.Equal("1234567-8", Assert.Single(participant.OrganizationIdentifier!).PidContent);
        Assert.Equal(
            "https://research.fi/en/results/infrastructure/ttv-infrastructure-910",
            infrastructure.InfraResearchfiURL!.En);
        await transaction.RollbackAsync();
    }

    private static InfrastructureOrganization Organization(
        int id,
        string name,
        params PersistentIdentifier[] pids) => new()
    {
        Id = id,
        NameEn = name,
        Pids = pids.ToList()
    };
}
