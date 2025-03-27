using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingDecision;
using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Organization = CSC.PublicApi.Service.Models.Organization.Organization;
namespace Repository.Tests;

public class FundingDecisionIndexRepositoryTest
{
    private readonly FundingDecisionIndexRepository _fundingDecisionIndexRepository;
    
    /// <summary>
    /// Unit test for verifying the behavior of the <see cref="FundingDecisionIndexRepository.PerformInMemoryOperations"/> method.
    /// </summary>
    [Fact]
    public void PerformInMemoryOperations_FundingCall_IsHandledCorrectly()
    {
        // Arrange
        var fundingDecisions = new List<object>
        {
            GetModel()
        };
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var organization1 = new Organization
        {
            Id = 123,
            OrganizationId = "organizationId123",
            NameFi = "Test Organization 1 nameFi",
            Pids = new List<PersistentIdentifier>
            {
                new PersistentIdentifier
                {
                    Type = "businessid",
                    Content = "1234567-8"
                }
            }
        };
        var organization2 = new Organization
        {
            Id = 234,
            OrganizationId = "organizationId234",
            NameFi = "Test Organization 2 nameFi",
            Pids = new List<PersistentIdentifier>
            {
                new PersistentIdentifier
                {
                    Type = "businessid",
                    Content = "2345678-9"
                }
            }
        };
        memoryCache.Set(MemoryCacheKeys.OrganizationById(123), organization1);
        memoryCache.Set(MemoryCacheKeys.OrganizationById(234), organization2);

        // Initialize repository with memory cache
        var _fundingDecisionIndexRepository = new FundingDecisionIndexRepository(null, null, memoryCache);

        // Act
        var resultFundingDecisions = _fundingDecisionIndexRepository.PerformInMemoryOperations(fundingDecisions);
        
        // Assert
        resultFundingDecisions.Should().NotBeNullOrEmpty();
        var fundingDecisionObject = fundingDecisions.FirstOrDefault();
        fundingDecisionObject.Should().NotBeNull().And.BeAssignableTo<FundingDecision>();
        var fundingDecision = (FundingDecision)fundingDecisionObject!;

        // Check that Funder is set correctly in SetFunder
        fundingDecision.Funder.Should().NotBeNull();
        fundingDecision.Funder.NameFi.Should().Be("Test Organization 1 nameFi");
        fundingDecision.Funder.Pids.Should().NotBeNullOrEmpty();
        fundingDecision.Funder.Pids[0].Type.Should().Be("businessid");
        fundingDecision.Funder.Pids[0].Content.Should().Be("1234567-8");

        // Check that funding receivers are set correctly in SetFundingReceivers
        fundingDecision.FundingReceivers.Should().NotBeNullOrEmpty();
        fundingDecision.FundingReceivers[0].FunderProjectNumber.Should().Be("funding group person 1 source id");
        fundingDecision.FundingReceivers[0].Organization.NameFi.Should().Be("Test Organization 1 nameFi");
        fundingDecision.FundingReceivers[0].Organization.Pids.Should().NotBeNullOrEmpty();
        fundingDecision.FundingReceivers[0].Organization.Pids[0].Type.Should().Be("businessid");
        fundingDecision.FundingReceivers[0].Organization.Pids[0].Content.Should().Be("1234567-8");
        fundingDecision.FundingReceivers[0].Person.FirstNames.Should().Be("funding group person 1 first name");
        fundingDecision.FundingReceivers[0].Person.LastName.Should().Be("funding group person 1 last name");
        fundingDecision.FundingReceivers[0].Person.OrcId.Should().Be("funding group person 1 orcid");
        fundingDecision.FundingReceivers[0].RoleInFundingGroup.Should().Be("funding group person 1 role");
        fundingDecision.FundingReceivers[0].ShareOfFundingInEur.Should().Be(123.45m);
        fundingDecision.FundingReceivers[1].FunderProjectNumber.Should().Be("funding group person 2 source id");
        fundingDecision.FundingReceivers[1].Organization.NameFi.Should().Be("Test Organization 2 nameFi");
        fundingDecision.FundingReceivers[1].Organization.Pids.Should().NotBeNullOrEmpty();
        fundingDecision.FundingReceivers[1].Organization.Pids[0].Type.Should().Be("businessid");
        fundingDecision.FundingReceivers[1].Organization.Pids[0].Content.Should().Be("2345678-9");
        fundingDecision.FundingReceivers[1].Person.FirstNames.Should().Be("funding group person 2 first name");
        fundingDecision.FundingReceivers[1].Person.LastName.Should().Be("funding group person 2 last name");
        fundingDecision.FundingReceivers[1].Person.OrcId.Should().Be("funding group person 2 orcid");
        fundingDecision.FundingReceivers[1].RoleInFundingGroup.Should().Be("funding group person 2 role");
        fundingDecision.FundingReceivers[1].ShareOfFundingInEur.Should().Be(234.56m);
    
        // Check that CallProgramme are set correctly in SetCallProgrammes
        fundingDecision.CallProgrammes.Should().NotBeNullOrEmpty();
        fundingDecision.CallProgrammes[0].Id.Should().Be(654321);
        fundingDecision.CallProgrammes[0].SourceId.Should().Be("Call programme source id");
        fundingDecision.CallProgrammes[0].NameFi.Should().Be("Call programme name fi");
        fundingDecision.CallProgrammes[0].NameSv.Should().Be("Call programme name sv");
        fundingDecision.CallProgrammes[0].NameEn.Should().Be("Call programme name en");
        fundingDecision.CallProgrammes[0].Abbreviation.Should().Be("Call programme abbreviation");
        fundingDecision.CallProgrammes[0].SourceDescription.Should().Be("Call programme source description");
        fundingDecision.CallProgrammes[0].SourceProgrammeId.Should().Be("Call programme source programme id");

        // Check that ResearchfiUrl is set correctly in SetResearchfiUrl
        fundingDecision.ResearchfiUrl.Should().NotBeNull();
        fundingDecision.ResearchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/funding/123456");
        fundingDecision.ResearchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/funding/123456");
        fundingDecision.ResearchfiUrl.En.Should().Be("https://research.fi/en/results/funding/123456");
    }
    
    
    private static FundingDecision GetModel()
    {
        return new FundingDecision
        {
            Id = 123456,
            TypeOfFunding = new ReferenceData
            {
                Code = "Type of funding code 1",
                NameFi = "Type of funding name fi 1",
                NameSv = "Type of funding name sv 1",
                NameEn = "Type of funding name en 1"
            },
            FunderId = 123,
            SelfFundingGroupPerson = new List<FundingGroupPerson>
            {
                new FundingGroupPerson
                {
                    SourceId = "funding group person 1 source id",
                    Person = new Person
                    {
                        FirstNames = "funding group person 1 first name",
                        LastName = "funding group person 1 last name",
                        OrcId = "funding group person 1 orcid"
                    },
                    RoleInFundingGroup = "funding group person 1 role",
                    OrganizationId = 123,
                    ShareOfFundingInEur = 123.45m
                }
            },
            ParentFundingGroupPerson = new List<FundingGroupPerson>
            {
                new FundingGroupPerson
                {
                    SourceId = "funding group person 2 source id",
                    Person = new Person
                    {
                        FirstNames = "funding group person 2 first name",
                        LastName = "funding group person 2 last name",
                        OrcId = "funding group person 2 orcid"
                    },
                    RoleInFundingGroup = "funding group person 2 role",
                    OrganizationId = 234,
                    ShareOfFundingInEur = 234.56m
                }
            },
            CallProgramme = new CallProgramme
            {
                Id = 654321,
                SourceId = "Call programme source id",
                NameFi = "Call programme name fi",
                NameSv = "Call programme name sv",
                NameEn = "Call programme name en",
                Abbreviation = "Call programme abbreviation",
                SourceDescription = "Call programme source description",
                SourceProgrammeId = "Call programme source programme id"
            },
            // ResearchfiUrl should be set in SetResearchfiUrl
        };
    }
}