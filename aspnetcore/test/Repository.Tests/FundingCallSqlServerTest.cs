using AutoMapper;
using CSC.PublicApi.Repositories;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models.FundingCall;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Repository.Tests;

[Collection(SqlServerRepositoryCollection.Name)]
[Trait("Category", "SqlServerIntegration")]
public sealed class FundingCallSqlServerTest(SqlServerRepositoryFixture fixture)
{
    private readonly IMapper _mapper = new MapperConfiguration(
        configuration => configuration.AddProfile<FundingCallProfile>()).CreateMapper();

    [Fact]
    public async Task GetAll_ExcludesSentinelClosedCallsAndCallsWithoutOrganizations()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingCall> repository = new FundingCallIndexRepository(context, _mapper, cache);

        var calls = await repository.GetAll().ToListAsync();

        calls.Select(call => call.Id).Should().BeEquivalentTo(new[] { 10, 40 });
        context.ChangeTracker.Entries().Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ProjectsDatesOrganizationsCategoriesAndLanguageSpecificLinks()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingCall> repository = new FundingCallIndexRepository(context, _mapper, cache);

        var calls = await repository.GetAll().ToListAsync();
        var call = calls.Single(call => call.Id == 10);

        call.ExportSortId.Should().Be(10);
        call.NameEn.Should().Be("Dated synthetic call");
        call.DescriptionEn.Should().Be("Synthetic description");
        call.ApplicationTermsEn.Should().Be("Synthetic terms");
        call.ContactInformation.Should().Be("Synthetic contact");
        call.SourceId.Should().Be("call-10");
        call.SourceDescription.Should().Be("synthetic");
        call.SourceProgrammeId.Should().Be("programme-10");
        call.Abbreviation.Should().Be("SYN");
        call.EuCallId.Should().Be("EU-SYN");
        call.CallProgrammeOpenDate.Should().Be(new DateTime(2026, 1, 15));
        call.CallProgrammeDueDate.Should().Be(new DateTime(2026, 2, 28));
        call.ContinuousApplication.Should().BeFalse();
        call.ApplicationUrlFi.Should().Be("https://example.invalid/apply/fi");
        call.ApplicationUrlSv.Should().Be("https://example.invalid/apply/sv");
        call.ApplicationUrlEn.Should().Be("https://example.invalid/apply/en");
        call.Foundations.Should().HaveCount(2);
        var foundation = call.Foundations!.Single(foundation => foundation.Id == 101);
        foundation.NameFi.Should().Be("Synteettinen rahasto");
        foundation.NameSv.Should().Be("Syntetisk fond");
        foundation.NameEn.Should().Be("Synthetic foundation");
        foundation.BusinessId.Should().Be("foundation-one");
        foundation.FoundationUrl.Should().Be("https://example.invalid/foundation");
        call.Categories!.Select(category => category.Code)
            .Should().BeEquivalentTo(new[] { "SYNTHETIC-A", "SYNTHETIC-B" });
        var category = call.Categories.Single(category => category.Code == "SYNTHETIC-A");
        category.NameFi.Should().Be("Ala A");
        category.NameSv.Should().Be("Kategori A");
        category.NameEn.Should().Be("Field A");
    }

    [Fact]
    public async Task GetAll_SentinelDatesProduceContinuousCallWithoutApplicationLinks()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingCall> repository = new FundingCallIndexRepository(context, _mapper, cache);

        var calls = await repository.GetAll().ToListAsync();
        var call = calls.Single(call => call.Id == 40);

        call.ContinuousApplication.Should().BeTrue();
        call.CallProgrammeOpenDate.Should().BeNull();
        call.CallProgrammeDueDate.Should().BeNull();
        call.ApplicationUrlFi.Should().BeNull();
        call.ApplicationUrlSv.Should().BeNull();
        call.ApplicationUrlEn.Should().BeNull();
        call.Categories.Should().BeEmpty();
        call.Foundations.Should().ContainSingle().Which.Id.Should().Be(101);
    }

    [Fact]
    public async Task GetChunk_PagesSourceRowsBeforeFilteringAndOrdersById()
    {
        await using var context = fixture.CreateContext();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        IIndexRepository<FundingCall> repository = new FundingCallIndexRepository(context, _mapper, cache);

        var first = await repository.GetChunk(0, 2).ToListAsync();
        var second = await repository.GetChunk(2, 2).ToListAsync();
        var third = await repository.GetChunk(4, 2).ToListAsync();
        var beyondEnd = await repository.GetChunk(6, 2).ToListAsync();

        first.Select(call => call.Id).Should().Equal(10);
        second.Should().BeEmpty();
        third.Select(call => call.Id).Should().Equal(40);
        beyondEnd.Should().BeEmpty();
    }
}
