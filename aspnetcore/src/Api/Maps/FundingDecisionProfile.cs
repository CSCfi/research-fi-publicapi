using Api.Models.Entities;
using Api.Models.FundingDecision;
using AutoMapper;

namespace Api.Maps
{
    public class FundingDecisionProfile : Profile
    {
        public FundingDecisionProfile()
        {
            CreateProjection<DimFundingDecision, FundingDecision>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.Acronym, opt => opt.MapFrom(src => src.Acronym))
                .ForMember(dst => dst.DescriptionFi, opt => opt.MapFrom(src => src.DescriptionFi))
                .ForMember(dst => dst.DescriptionSv, opt => opt.MapFrom(src => src.DescriptionSv))
                .ForMember(dst => dst.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
                .ForMember(dst => dst.FundingStartYear, opt => opt.MapFrom(src => src.DimDateIdStartNavigation))
                .ForMember(dst => dst.FundingEndYear, opt => opt.MapFrom(src => src.DimDateIdEndNavigation))
                .ForMember(dst => dst.FundingGroupPerson, opt => opt.MapFrom(src => src.BrParticipatesInFundingGroups))
                .ForMember(dst => dst.OrganizationConsortia, opt => opt.MapFrom(src => src.BrFundingConsortiumParticipations))
                .ForMember(dst => dst.Funder, opt => opt.MapFrom(src => src.DimOrganizationIdFunderNavigation))
                .ForMember(dst => dst.TypeOfFunding, opt => opt.MapFrom(src => src.DimTypeOfFunding))
                .ForMember(dst => dst.CallProgramme, opt => opt.MapFrom(src => src.SourceDescription != "eu_funding" ? src.DimCallProgramme : null))
                .ForMember(dst => dst.FunderProjectNumber, opt => opt.MapFrom(src => src.FunderProjectNumber))
                .ForMember(dst => dst.FieldsOfScience, opt => opt.MapFrom(src => src.DimFieldOfSciences.Where(fieldOfScience => fieldOfScience.Id != -1)))
                .ForMember(dst => dst.Keywords, opt => opt.MapFrom(src => src.DimKeywords.Where(kw => kw.Scheme == "Tutkimusala")))
                .ForMember(dst => dst.IdentifiedTopics, opt => opt.MapFrom(src => src.BrWordClusterDimFundingDecisions.SelectMany(x => x.DimWordCluster.BrWordsDefineAClusters)))
                .ForMember(dst => dst.AmountInEur, opt => opt.MapFrom(src => src.AmountInEur))
                .ForMember(dst => dst.Topic, opt => opt.MapFrom(src => src.SourceDescription == "eu_funding" ? src.DimCallProgramme : null))
                // TODO: ugly, probably have to convert to direct sql query.
                // Tries to find CallProgramme's FrameworkProgramme by checking if CallProgramme's DimCallProgrammeId2s contain another CallProgramme which we consider a parent and repeat the search process for it.
                // Have to keep eye on the generated sql query and performance.
                // Note that these long chains of commands can cause null reference exceptions easily in unit tests but in linq-to-entities (sql) they will not throw.
                .ForMember(dst => dst.FrameworkProgramme, opt =>
                    opt.MapFrom(src =>
                        src.DimCallProgramme.DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1) != null
                            ? src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1) != null
                                ? src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1) != null
                                    ? src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1) != null
                                        ? src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1) != null
                                            ? src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.SingleOrDefault(cp => cp.Id != -1) != null
                                                ? src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single()
                                                : src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single()
                                            : src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single()
                                        : src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single()
                                    : src.DimCallProgramme.DimCallProgrammeId2s.Single().DimCallProgrammeId2s.Single()
                                : src.DimCallProgramme.DimCallProgrammeId2s.Single()
                            : null));

            CreateProjection<DimDate, int?>()
                .ConvertUsing(x => x != null ? x.Year : null);

            CreateProjection<BrParticipatesInFundingGroup, FundingGroupPerson>()
                .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.DimName.LastName))
                .ForMember(dst => dst.FirstNames, opt => opt.MapFrom(src => src.DimName.FirstNames))
                .ForMember(dst => dst.OrcId, opt => opt.MapFrom(src => src.DimName.DimKnownPersonIdConfirmedIdentityNavigation))
                .ForMember(dst => dst.RoleInFundingGroup, opt => opt.MapFrom(src => src.RoleInFundingGroup));

            CreateProjection<DimKnownPerson, string?>()
                .ConvertUsing(x => x.DimPids.Where(p => p.PidType == "Orcid")
                                            .Select(p => p.PidContent)
                                            .SingleOrDefault());

            CreateProjection<BrFundingConsortiumParticipation, OrganizationConsortium>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.DimOrganization.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.DimOrganization.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.DimOrganization.NameEn))
                .ForMember(dst => dst.BusinessId, opt => opt.MapFrom(src => src.DimOrganization.DimPids.SingleOrDefault(p => p.PidType == "BusinessID")))
                .ForMember(dst => dst.RoleInConsortium, opt => opt.MapFrom(src => src.RoleInConsortium))
                .ForMember(dst => dst.ShareOfFundingInEur, opt => opt.MapFrom(src => src.ShareOfFundingInEur))
                .ForMember(dst => dst.Pic, opt => opt.MapFrom(src => src.DimOrganization.DimPids.SingleOrDefault(p => p.PidType == "PIC")))
                .ForMember(dst => dst.IsFinnishOrganization, opt => opt.MapFrom(src => src.DimOrganization.DimPids.Any(p => p.PidType == "BusinessID")));

            // Used by
            //  OrganizationConsortium.BusinessID
            //  OrganizationConsortium.PIC
            CreateProjection<DimPid, string?>()
                .ConvertUsing(pid => pid.PidContent);

            CreateProjection<DimOrganization, Funder>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.Ids, opt => opt.MapFrom(src => src.DimPids));

            CreateProjection<DimTypeOfFunding, FundingType>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => src.TypeId));

            CreateProjection<DimCallProgramme, CallProgramme>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.CallProgrammeId, opt => opt.MapFrom(src => src.SourceId));

            CreateProjection<DimFieldOfScience, FieldOfScience>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.FieldId, opt => opt.MapFrom(src => src.FieldId));

            CreateProjection<DimKeyword, string>()
                .ConvertUsing(keyword => keyword.Keyword);

            CreateProjection<DimCallProgramme, Topic>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dst => dst.EuCallId, opt => opt.MapFrom(src => src.EuCallId))
                .ForMember(dst => dst.TopicId, opt => opt.MapFrom(src => src.Abbreviation));

            CreateProjection<BrWordsDefineACluster, string>()
                .ConvertUsing(x => x.DimMinedWords.Word);

            CreateProjection<DimPid, Id>()
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PidType))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.PidContent));

            CreateProjection<DimCallProgramme, FrameworkProgramme>()
                .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
                .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
                .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));
        }
    }
}
