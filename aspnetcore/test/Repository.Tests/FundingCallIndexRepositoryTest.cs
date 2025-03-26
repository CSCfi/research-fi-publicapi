using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingCall;
using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using CSC.PublicApi.Service.Models.Organization;
namespace Repository.Tests;

public class FundingCallIndexRepositoryTest
{
    private readonly FundingCallIndexRepository _fundingCallIndexRepository;
    
    /// <summary>
    /// Unit test for verifying the behavior of the <see cref="FundingCallIndexRepository.PerformInMemoryOperations"/> method.
    /// </summary>
    [Fact]
    public void PerformInMemoryOperations_FundingCall_IsHandledCorrectly()
    {
        // Arrange
        var fundingCalls = new List<object>
        {
            GetModel()
        };
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        // In index repository, property BusinessId of a foundation (FundingCall.Foundations) is set in function HandleFoundationBusinessID.
        // HandleFoundationBusinessID searches data from organization memory cache. Add test organizations to cache.
        var organization1 = new Organization
        {
            Id = 111, // This id must match foundation id in the model
            NameFi = "Test Organization 1 nameFi",
            Pids = new List<PersistentIdentifier>
            {
                new PersistentIdentifier
                {
                    Type = "foo1",
                    Content = "bar1"
                },
                new PersistentIdentifier
                {
                    Type = "businessid",
                    Content = "1234567-8" // In HandleFoundationBusinessID, this should be set to property BusinessId
                }
            }
        };
        var organization2 = new Organization
        {
            Id = 222, // This id must match foundation id in the model
            NameFi = "Test Organization 2 nameFi",
            Pids = new List<PersistentIdentifier>
            {
                new PersistentIdentifier
                {
                    Type = "businessid",
                    Content = "2345678-9" // In HandleFoundationBusinessID, this should be set to property BusinessId
                },
                new PersistentIdentifier
                {
                    Type = "foo2",
                    Content = "bar2"
                }
            }
        };
        memoryCache.Set(MemoryCacheKeys.OrganizationById(111), organization1);
        memoryCache.Set(MemoryCacheKeys.OrganizationById(222), organization2);

        // Initialize repository with memory cache
        var _fundingCallIndexRepository = new FundingCallIndexRepository(null, null, memoryCache);

        // Act
        var resultFundingCalls = _fundingCallIndexRepository.PerformInMemoryOperations(fundingCalls);
        
        // Assert
        resultFundingCalls.Should().NotBeNullOrEmpty();
        var fundingCallObject = fundingCalls.FirstOrDefault();
        fundingCallObject.Should().NotBeNull().And.BeAssignableTo<FundingCall>();
        var fundingCall = (FundingCall)fundingCallObject!;
        fundingCall.Foundations[0].BusinessId.Should().Be("1234567-8");
        fundingCall.Foundations[1].BusinessId.Should().Be("2345678-9");
    }
    
    
    private static FundingCall GetModel()
    {
        return new FundingCall
        {
            NameFi = "Test Name Fi",
            NameSv = "Test Name Sv",
            NameEn = "Test Name En",
            DescriptionFi = "Test Description Fi",
            DescriptionSv = "Test Description Sv",
            DescriptionEn = "Test Description En",
            ApplicationTermsFi = "Test Application Terms Fi",
            ApplicationTermsSv = "Test Application Terms Sv",
            ApplicationTermsEn = "Test Application Terms En",
            ContinuousApplication = true,
            CallProgrammeOpenDate = DateTime.UtcNow.AddDays(-10),
            CallProgrammeDueDate = DateTime.UtcNow.AddDays(10),
            ApplicationUrlFi = "https://example.com/fi",
            ApplicationUrlSv = "https://example.com/sv",
            ApplicationUrlEn = "https://example.com/en",
            Foundations = new List<Foundation>
            {
            new Foundation
            {
                Id = 111,
                NameFi = "Test Foundation nameFi 1",
                NameEn = "Test Foundation nameEn 1",
                NameSv = "Test Foundation nameSv 1",
                FoundationUrl = "https://foundation1.com"
                // BusinessId should be set in HandleFoundationBusinessID
            },
            new Foundation
            {
                Id = 222,
                NameFi = "Test Foundation nameFi 2",
                NameEn = "Test Foundation nameEn 2",
                NameSv = "Test Foundation nameSv 2",
                FoundationUrl = "https://foundation2.com"
                // BusinessId should be set in HandleFoundationBusinessID
            }
            },
            Categories = new List<ReferenceData>
            {
                new ReferenceData { Code = "CAT1", NameFi = "Category 1 Fi", NameSv = "Category 1 Sv", NameEn = "Category 1 En" },
                new ReferenceData { Code = "CAT2", NameFi = "Category 2 Fi", NameSv = "Category 2 Sv", NameEn = "Category 2 En" }
            },
            ContactInformation = "Test Contact Information",
            ResearchfiUrl = new ResearchfiUrl(resourceType: "funding-call", id: "123"),
            Id = 123,
            Abbreviation = "TEST",
            SourceId = "SRC123",
            SourceDescription = "Test Source Description",
            EuCallId = "EU123",
            SourceProgrammeId = "PROG123"
        };
    }
}