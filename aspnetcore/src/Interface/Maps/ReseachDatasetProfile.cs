using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.ResearchDataset;
using Version = CSC.PublicApi.Interface.Models.ResearchDataset.Version;

namespace CSC.PublicApi.Interface.Maps;

public class ReseachDatasetProfile : Profile
{
    public ReseachDatasetProfile()
    {
        CreateMap<GetResearchDatasetsQueryParameters, ResearchDatasetSearchParameters>();
        CreateMap<Service.Models.ResearchDataset.ResearchDataset, ResearchDataset>();
        CreateMap<Service.Models.ResearchDataset.FieldOfScience, FieldOfScience>();
        CreateMap<Service.Models.ResearchDataset.Keyword, Keyword>();
        CreateMap<Service.Models.ResearchDataset.Language, Language>();
        CreateMap<Service.Models.ResearchDataset.License, License>();
        CreateMap<Service.Models.ResearchDataset.PreferredIdentifier, PreferredIdentifier>();
        CreateMap<Service.Models.ResearchDataset.Version, Version>();
    }
}