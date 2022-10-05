using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.FundingDecision;

namespace CSC.PublicApi.Interface.Maps;

public class FundingDecisionProfile : Profile
{
    public FundingDecisionProfile()
    {
        CreateMap<GetFundingCallQueryParameters, FundingDecisionSearchParameters>();
        CreateMap<Service.Models.FundingDecision.FundingDecision, FundingDecision>();
        CreateMap<Service.Models.FundingDecision.CallProgramme, CallProgramme>();
        CreateMap<Service.Models.FundingDecision.ContactPerson, ContactPerson>();
        CreateMap<Service.Models.FundingDecision.Contribution, Contribution>();
        CreateMap<Service.Models.FundingDecision.Date, Date>();
        CreateMap<Service.Models.FundingDecision.FieldOfScience, FieldOfScience>();
        CreateMap<Service.Models.FundingDecision.FrameworkProgramme, FrameworkProgramme>();
        CreateMap<Service.Models.FundingDecision.Funder, Funder>();
        CreateMap<Service.Models.FundingDecision.FundingGroupPerson, FundingGroupPerson>();
        CreateMap<Service.Models.FundingDecision.FundingType, FundingType>();
        CreateMap<Service.Models.FundingDecision.Geo, Geo>();
        CreateMap<Service.Models.FundingDecision.Id, Id>();
        CreateMap<Service.Models.FundingDecision.Infrastructure, Infrastructure>();
        CreateMap<Service.Models.FundingDecision.Keyword, Keyword>();
        CreateMap<Service.Models.FundingDecision.OrganizationConsortium, OrganizationConsortium>();
        CreateMap<Service.Models.FundingDecision.Publication, Publication>();
        CreateMap<Service.Models.FundingDecision.Sector, Sector>();
        CreateMap<Service.Models.FundingDecision.Topic, Topic>();
    }
}