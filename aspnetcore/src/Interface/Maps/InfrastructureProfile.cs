using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Infrastructure;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateMap<GetInfrastructuresQueryParameters, InfrastructureSearchParameters>();
        CreateMap<Service.Models.Infrastructure.Infrastructure, Infrastructure>();
        CreateMap<Service.Models.ResearchfiUrl, ResearchFi.ResearchfiUrl>();

        CreateMap<Service.Models.Infrastructure.Identifier, Identifier>();
        CreateMap<Service.Models.Infrastructure.PidAttributes, PidAttributes>();
        CreateMap<Service.Models.Infrastructure.InfrastructureNetwork, InfrastructureNetwork>();
        CreateMap<Service.Models.Infrastructure.InternationalInfra, InternationalInfra>();
        CreateMap<Service.Models.Infrastructure.ResearchOrganization, ResearchOrganization>();
        CreateMap<Service.Models.Infrastructure.InfrastructureService, InfrastructureService>();
        CreateMap<Service.Models.Infrastructure.Weblink, Weblink>();
        CreateMap<Service.Models.Infrastructure.DescriptiveText, DescriptiveText>();
        CreateMap<Service.Models.Infrastructure.ReferenceData, ReferenceData>();
        CreateMap<Service.Models.Infrastructure.ContactInformation, ContactInformation>();
        CreateMap<Service.Models.Infrastructure.Address, Address>();
        CreateMap<Service.Models.Infrastructure.InfraDate, InfraDate>();
        CreateMap<Service.Models.Infrastructure.LanguageVariant, LanguageVariant>();
    }
}