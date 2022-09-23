using AutoMapper;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CSC.PublicApi.Interface.Models.ResearchDataset;
using CSC.PublicApi.DataAccess.Entities;
using CSC.PublicApi.DataAccess.Maps;

namespace CSC.PublicApi.Tests.Maps;

public class ResearchDatasetProfileTest
{
    private readonly IMapper _mapper;

    public ResearchDatasetProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ResearchDatasetProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_DimCallProgramme_To_FundingCall()
    {
        // Arrange
        var entity = GetEntity();

        // Act
        var model = Act_Map(entity);

        // Assert
        model.Should().BeEquivalentTo(GetModel());
    }

    private ResearchDataset Act_Map(DimResearchDataset dbEntity)
    {
        var entityQueryable = new List<DimResearchDataset>
        {
            dbEntity
        }.AsQueryable();

        var modelCollection = _mapper.ProjectTo<ResearchDataset>(entityQueryable);

        return modelCollection.Single();
    }

    private static DimResearchDataset GetEntity()
    {
        return new DimResearchDataset
        {
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            DatasetCreated = new System.DateTime(2021, 10, 1),
            DimReferencedataAvailabilityNavigation = new()
            {
                CodeValue = "access type",
                CodeScheme = "access_type"
            },
            DimReferencedataLicenseNavigation = new()
            {
                CodeValue = "license code",
                CodeScheme = "license",
                NameFi = "nameFi",
                NameSv = "nameSv",
                NameEn = "nameEn",
            },
            LocalIdentifier = "localID"

        };
    }

    private static ResearchDataset GetModel()
    {
        return new ResearchDataset
        {
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            DatasetCreated = new System.DateTime(2021, 10, 1),
            AccessType = "access type",
            License = new()
            {
                Code = "license code",
                NameFi = "nameFi",
                NameSv = "nameSv",
                NameEn = "nameEn",
            },
            LocalIdentifier = "localID",
            FairDataUrl = "https://etsin.fairdata.fi/dataset/localID"
        };
    }
}