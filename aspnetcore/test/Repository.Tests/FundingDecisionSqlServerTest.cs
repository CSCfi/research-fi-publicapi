using AutoMapper;
using CSC.PublicApi.Repositories;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models.FundingDecision;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Repository.Tests;

[Collection(SqlServerRepositoryCollection.Name)]
[Trait("Category", "SqlServerIntegration")]
public sealed class FundingDecisionSqlServerTest(SqlServerRepositoryFixture fixture)
{
    private readonly IMapper _mapper = new MapperConfiguration(
        configuration => configuration.AddProfile<FundingDecisionProfile>()).CreateMapper();

    [Fact]
    public async Task GetAll_RequiresConsortiumPicOnlyForEuFundingAndExcludesSentinel()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);

        var decisions = await repository.GetAll().ToListAsync();

        decisions.Select(decision => decision.Id).Should().BeEquivalentTo(new[] { 100, 110, 150, 160, 170 });
        context.ChangeTracker.Entries().Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ProjectsDecisionFieldsDatesAndRelatedCollections()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);

        var decisions = await repository.GetAll().ToListAsync();
        var decision = decisions.Single(decision => decision.Id == 110);

        decision.ExportSortId.Should().Be(110);
        decision.NameFi.Should().Be("Synteettinen paatos");
        decision.NameSv.Should().Be("Syntetiskt beslut");
        decision.NameEn.Should().Be("EU decision with consortium PIC");
        decision.Acronym.Should().Be("SYN");
        decision.DescriptionFi.Should().Be("Synteettinen kuvaus");
        decision.DescriptionSv.Should().Be("Syntetisk beskrivning");
        decision.DescriptionEn.Should().Be("Synthetic description");
        decision.FunderId.Should().Be(101);
        decision.FunderProjectNumber.Should().Be("SYN-110");
        decision.AmountInEur.Should().Be(12345.67m);
        decision.SourceId.Should().Be("decision-110");
        decision.SourceDescription.Should().Be("eu_funding");
        decision.Created.Should().Be(new DateTime(2026, 1, 1));
        decision.Modified.Should().Be(new DateTime(2026, 1, 2));
        decision.FundingStartDate.Should().Be(new DateTime(2026, 1, 15));
        decision.FundingEndDate.Should().Be(new DateTime(2026, 2, 28));
        decision.TypeOfFunding!.Code.Should().Be("SYNTHETIC-A");
        decision.FieldsOfScience!.Select(field => field.Code)
            .Should().BeEquivalentTo(new[] { "SYNTHETIC-A", "SYNTHETIC-B" });
        var keyword = decision.Keywords.Should().ContainSingle().Which;
        keyword.Value.Should().Be("Synthetic research field");
        keyword.Language.Should().Be("en");
        keyword.Scheme.Should().Be("Tutkimusala");
        decision.IdentifiedTopics.Should().BeEquivalentTo(new[] { "synthetic topic one", "synthetic topic two" });
        decision.CallProgramme!.Id.Should().Be(10);
        decision.CallProgrammeParent1!.Id.Should().Be(20);
        decision.CallProgrammeParent2!.Id.Should().Be(30);
        decision.CallProgrammeParent3!.NameEn.Should().Be("Continuous synthetic call");
        decision.CallProgrammeParent4.Should().BeNull();
        decision.CallProgrammeParent5.Should().BeNull();
        decision.CallProgrammeParent6.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ProjectsSelfAndParentContributorsAndConsortiumShares()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);

        var decisions = await repository.GetAll().ToListAsync();
        var decision = decisions.Single(decision => decision.Id == 110);

        var self = decision.SelfFundingGroupPerson.Should().ContainSingle().Which;
        self.SourceId.Should().Be("decision-110");
        self.OrganizationId.Should().Be(101);
        self.RoleInFundingGroup.Should().Be("child role");
        self.ShareOfFundingInEur.Should().Be(234.56m);
        self.Person!.FirstNames.Should().Be("Synthetic child");
        self.Person.LastName.Should().Be("Contributor");
        self.Person.OrcId.Should().Be("synthetic-child-orcid");
        var parent = decision.ParentFundingGroupPerson.Should().ContainSingle().Which;
        parent.SourceId.Should().Be("parent-contribution");
        parent.OrganizationId.Should().Be(102);
        parent.RoleInFundingGroup.Should().Be("parent role");
        parent.ShareOfFundingInEur.Should().Be(100.25m);
        parent.Person!.FirstNames.Should().Be("Synthetic parent");
        parent.Person.OrcId.Should().Be("synthetic-parent-orcid");
        var consortium = decision.OrganizationConsortia.Should().ContainSingle().Which;
        consortium.OrganizationId.Should().Be(101);
        consortium.RoleInConsortium.Should().Be("coordinator");
        consortium.ShareOfFundingInEur.Should().Be(12345.67m);

        var mixedConsortium = decisions.Single(decision => decision.Id == 150).OrganizationConsortia!;
        mixedConsortium.Select(member => member.OrganizationId).Should().BeEquivalentTo(new[] { 101, 102 });
        mixedConsortium.Sum(member => member.ShareOfFundingInEur).Should().Be(200.50m);
    }

    [Fact]
    public async Task GetAll_ProjectsSentinelDatesAndMissingOptionalRelationships()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);

        var decisions = await repository.GetAll().ToListAsync();
        var decision = decisions.Single(decision => decision.Id == 170);

        decision.FundingStartDate.Should().BeNull();
        decision.FundingEndDate.Should().BeNull();
        decision.FunderId.Should().BeNull();
        decision.SelfFundingGroupPerson.Should().BeEmpty();
        decision.ParentFundingGroupPerson.Should().BeEmpty();
        decision.OrganizationConsortia.Should().BeEmpty();
        decision.Keywords.Should().BeEmpty();
        decision.FieldsOfScience.Should().BeEmpty();
        decision.IdentifiedTopics.Should().BeEmpty();
        decision.CallProgramme!.Id.Should().Be(-1);
        decision.CallProgrammeParent1.Should().BeNull();
        decision.CallProgrammeParent6.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ProjectedRelationshipsFeedRepositoryEnrichment()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        foreach (var id in new[] { 101, 102 })
        {
            cache.Set(MemoryCacheKeys.OrganizationById(id),
                new CSC.PublicApi.Service.Models.Organization.Organization
                {
                    Id = id,
                    OrganizationId = $"synthetic-organization-{id}",
                    NameEn = $"Synthetic organization {id}"
                });
        }
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);
        var decisions = await repository.GetAll().ToListAsync();
        var decision = decisions.Single(decision => decision.Id == 110);

        repository.PerformInMemoryOperations(new List<object> { decision });

        decision.Funder!.NameEn.Should().Be("Synthetic organization 101");
        decision.FundingReceivers.Should().HaveCount(3);
        var self = decision.FundingReceivers!.Single(receiver => receiver.FunderProjectNumber == "decision-110");
        self.Person!.FundedPerson.Should().BeTrue();
        self.Organization!.NameEn.Should().Be("Synthetic organization 101");
        self.ShareOfFundingInEur.Should().Be(234.56m);
        var parent = decision.FundingReceivers.Single(receiver => receiver.FunderProjectNumber == "parent-contribution");
        parent.Person!.FundedPerson.Should().BeFalse();
        parent.Organization!.NameEn.Should().Be("Synthetic organization 102");
        var consortium = decision.FundingReceivers.Single(receiver => receiver.RoleInFundingGroup == "coordinator");
        consortium.Organization!.NameEn.Should().Be("Synthetic organization 101");
        consortium.ShareOfFundingInEur.Should().Be(12345.67m);
        decision.FrameworkProgramme!.NameEn.Should().Be("Continuous synthetic call");
        decision.CallProgrammes.Should().ContainSingle().Which.Id.Should().Be(10);
        decision.ResearchfiUrl!.En.Should().Be("https://research.fi/en/results/funding/110");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetAll_NullWarehouseTimestampCurrentlyFailsWithSqlConversionError(bool nullCreated)
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);
        if (nullCreated)
        {
            await context.Database.ExecuteSqlRawAsync(
                "UPDATE dim_funding_decision SET created = NULL WHERE id = 170");
        }
        else
        {
            await context.Database.ExecuteSqlRawAsync(
                "UPDATE dim_funding_decision SET modified = NULL WHERE id = 170");
        }

        var query = () => repository.GetAll().ToListAsync();

        await query.Should().ThrowAsync<SqlException>().Where(exception => exception.Number == 242);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetChunk_PagesSourceRowsBeforeApplyingEuEligibility()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingDecision> repository = new FundingDecisionIndexRepository(context, _mapper, cache);

        var first = await repository.GetChunk(0, 3).ToListAsync();
        var excluded = await repository.GetChunk(3, 3).ToListAsync();
        var last = await repository.GetChunk(6, 3).ToListAsync();
        var beyondEnd = await repository.GetChunk(9, 3).ToListAsync();

        first.Select(decision => decision.Id).Should().Equal(100, 110);
        excluded.Should().BeEmpty();
        last.Select(decision => decision.Id).Should().Equal(150, 160, 170);
        beyondEnd.Should().BeEmpty();
    }
}
