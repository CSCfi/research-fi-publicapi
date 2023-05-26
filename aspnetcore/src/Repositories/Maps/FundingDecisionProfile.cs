using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingDecision;
using Keyword = CSC.PublicApi.Service.Models.Keyword;
using Organization = CSC.PublicApi.Service.Models.FundingDecision.Organization;

namespace CSC.PublicApi.Repositories.Maps;

public class FundingDecisionProfile : Profile
{
    private const string FieldOfResearchKeywordScheme = "Tutkimusala";

    public FundingDecisionProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateProjection<DimFundingDecision, FundingDecision>()
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.Acronym, opt => opt.MapFrom(src => src.Acronym))
            .ForMember(dst => dst.DescriptionFi, opt => opt.MapFrom(src => src.DescriptionFi))
            .ForMember(dst => dst.DescriptionSv, opt => opt.MapFrom(src => src.DescriptionSv))
            .ForMember(dst => dst.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
            .ForMember(dst => dst.FundingStartDate, opt => opt.MapFrom(src => src.DimDateIdStartNavigation))
            .ForMember(dst => dst.FundingEndDate, opt => opt.MapFrom(src => src.DimDateIdEndNavigation))
            .ForMember(dst => dst.SelfFundingGroupPerson, opt => opt.MapFrom(src => src.BrParticipatesInFundingGroups))
            .ForMember(dst => dst.ParentFundingGroupPerson, opt => opt.MapFrom(src => src.DimFundingDecisionIdParentDecisionNavigation.BrParticipatesInFundingGroups))
            .ForMember(dst => dst.OrganizationConsortia, opt => opt.MapFrom(src => src.BrFundingConsortiumParticipations))
            .ForMember(dst => dst.Funder, opt => opt.MapFrom(src => src.DimOrganizationIdFunderNavigation))
            .ForMember(dst => dst.TypeOfFunding, opt => opt.MapFrom(src => src.DimTypeOfFunding))
            .ForMember(dst => dst.CallProgramme, opt => opt.MapFrom(src =>src.DimCallProgramme))
            .ForMember(dst => dst.FunderProjectNumber, opt => opt.MapFrom(src => src.FunderProjectNumber))
            .ForMember(dst => dst.FieldsOfScience, opt => opt.MapFrom(src => src.FactDimReferencedataFieldOfSciences))
            .ForMember(dst => dst.Keywords, opt => opt.MapFrom(src => src.DimKeywords.Where(kw => kw.Scheme == FieldOfResearchKeywordScheme)))
            .ForMember(dst => dst.IdentifiedTopics, opt => opt.MapFrom(src => src.BrWordClusterDimFundingDecisions.SelectMany(x => x.DimWordCluster.BrWordsDefineAClusters)))
            .ForMember(dst => dst.AmountInEur, opt => opt.MapFrom(src => src.AmountInEur))
            .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
            .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
            // Finds the parents of CallProgrammes for FrameworkProgramme. Deepest parent will be later copied to FrameworkProgramme in the index repository in memory operations.
            // Note that these long chains of expressions can cause null reference exceptions easily in unit tests but in linq-to-entities (sql) they will not throw.
            .ForMember(dst => dst.CallProgrammeParent1, opt => opt.MapFrom(src => src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1)))
            .ForMember(dst => dst.CallProgrammeParent2, opt => opt.MapFrom(src => src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1)))
            .ForMember(dst => dst.CallProgrammeParent3, opt => opt.MapFrom(src => src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1)))
            .ForMember(dst => dst.CallProgrammeParent4, opt => opt.MapFrom(src => src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1)))
            .ForMember(dst => dst.CallProgrammeParent5, opt => opt.MapFrom(src => src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1)))
            .ForMember(dst => dst.CallProgrammeParent6, opt => opt.MapFrom(src => src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1).DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1)))
            .ForMember(dst => dst.FrameworkProgramme, opt => opt.Ignore()) // FrameworkProgramme is populated later in memory from deepest CallProgrammeParent{X}, see above.
            .ForMember(dst => dst.Topic, opt => opt.Ignore()) // Topic will be included only for EU funding projects, where source_description is eu_funding.
            .ForMember(dst => dst.CallProgrammes, opt => opt.Ignore()) // CallProgrammes will be populated during the in memory operations
            .ForMember(dst => dst.FundingReceivers, opt => opt.Ignore()) // GrantedFunding will be populated during the in memory operations;
            ;
        
        CreateProjection<DimDate, DateTime?>()
            .ConvertUsing(dimDate => dimDate.Id == -1 ? null : new DateTime(dimDate.Year, dimDate.Month, dimDate.Day));

        CreateProjection<BrParticipatesInFundingGroup, FundingGroupPerson>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.SourceId, opt => opt.MapFrom(src => src.SourceId))
            .ForMember(dst => dst.Person, opt => opt.MapFrom(src => src.DimName))
            .ForMember(dst => dst.Organization, opt => opt.MapFrom(src => src.DimOrganization))
            .ForMember(dst => dst.ShareOfFundingInEur, opt => opt.MapFrom(src => src.ShareOfFundingInEur))
            .ForMember(dst => dst.RoleInFundingGroup, opt => opt.MapFrom(src => src.RoleInFundingGroup));


        CreateProjection<DimName, Person>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.FirstNames, opt => opt.MapFrom(src => src.FirstNames))
            .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dst => dst.OrcId, opt => opt.MapFrom(src => src.DimKnownPersonIdConfirmedIdentityNavigation));

        CreateProjection<DimKnownPerson, string?>()
            .ConvertUsing(x => x.DimPids.Where(p => p.PidType == "Orcid")
                .Select(p => p.PidContent)
                .SingleOrDefault());

        CreateProjection<BrFundingConsortiumParticipation, OrganizationConsortium>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Organization, opt => opt.MapFrom(src => src.DimOrganization))
            .ForMember(dst => dst.RoleInConsortium, opt => opt.MapFrom(src => src.RoleInConsortium))
            .ForMember(dst => dst.ShareOfFundingInEur, opt => opt.MapFrom(src => src.ShareOfFundingInEur));

        CreateProjection<DimOrganization, Organization>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids.Where(id => id.PidType == "BusinessID" || id.PidType == "PIC")))
            .ForMember(dst => dst.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
            .ForMember(dst => dst.IsFinnishOrganization, opt => opt.MapFrom(src => src.DimPids.Any(p => p.PidType == "BusinessID")));

        CreateProjection<DimCallProgramme, CallProgramme>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.SourceId, opt => opt.MapFrom(src => src.SourceId))
            .ForMember(dst => dst.SourceDescription, opt => opt.MapFrom(src => src.SourceDescription))
            .ForMember(dst => dst.SourceProgrammeId, opt => opt.MapFrom(src => src.SourceProgrammeId));
        
        CreateProjection<DimCallProgramme, FrameworkProgramme>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));
        
        CreateProjection<DimCallProgramme, Topic>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.EuCallId, opt => opt.MapFrom(src => src.EuCallId))
            .ForMember(dst => dst.TopicId, opt => opt.MapFrom(src => src.Abbreviation));

        CreateProjection<DimTypeOfFunding, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.TypeId));
        
        CreateProjection<FactDimReferencedataFieldOfScience, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.DimReferencedata.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.DimReferencedata.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.DimReferencedata.NameEn))
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.DimReferencedata.CodeValue));

        CreateProjection<DimKeyword, Keyword>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Keyword))
            .ForMember(dst => dst.Language, opt => opt.MapFrom(src => src.Language))
            .ForMember(dst => dst.Scheme, opt => opt.MapFrom(src => src.Scheme));

        CreateProjection<BrWordsDefineACluster, string>()
            .ConvertUsing(x => x.DimMinedWords.Word);

        CreateProjection<DimPid, PersistentIdentifier>()
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PidType))
            .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.PidContent));
    }
}