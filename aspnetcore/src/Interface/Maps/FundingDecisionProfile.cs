using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi;
using ResearchFi.CodeList;
using ResearchFi.FundingDecision;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class FundingDecisionProfile : Profile
{
    public FundingDecisionProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateMap<GetFundingDecisionQueryParameters, FundingDecisionSearchParameters>();
        
        CreateMap<Service.Models.FundingDecision.FundingDecision, FundingDecision>();
        CreateMap<Service.Models.FundingDecision.CallProgramme, CallProgramme>()
            .ForMember(dst => dst.CallProgrammeId, opt => opt.MapFrom(src => src.SourceId));
        CreateMap<Service.Models.FundingDecision.Topic, Topic>();
        CreateMap<Service.Models.FundingDecision.FrameworkProgramme, FrameworkProgramme>();
        CreateMap<Service.Models.FundingDecision.Funder, Funder>();
        CreateMap<Service.Models.FundingDecision.FundingGroupPerson, FundingGroupPerson>();
        CreateMap<Service.Models.FundingDecision.OrganizationConsortium, OrganizationConsortium>();
        CreateMap<Service.Models.PersistentIdentifier, PersistentIdentifier>();
        CreateMap<Service.Models.ReferenceData, FundingType>();
        CreateMap<Service.Models.ReferenceData, FieldOfScience>();
        CreateMap<Service.Models.Keyword, Keyword>();
    }
}