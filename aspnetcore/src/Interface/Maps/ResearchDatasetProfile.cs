using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.ResearchDataset;
using Version = CSC.PublicApi.Interface.Models.ResearchDataset.Version;

namespace CSC.PublicApi.Interface.Maps;

public class ResearchDatasetProfile : Profile
{
    public ResearchDatasetProfile()
    {
        CreateMap<GetResearchDatasetsQueryParameters, ResearchDatasetSearchParameters>();
        CreateMap<Service.Models.ResearchDataset.ResearchDataset, ResearchDataset>();
        CreateMap<Service.Models.ResearchDataset.FieldOfScience, FieldOfScience>();
        CreateMap<Service.Models.ResearchDataset.Keyword, Keyword>();
        CreateMap<Service.Models.ResearchDataset.Language, Language>();
        CreateMap<Service.Models.ResearchDataset.License, License>();
        CreateMap<Service.Models.ResearchDataset.PreferredIdentifier, PreferredIdentifier>();
        CreateMap<Service.Models.ResearchDataset.Availability, Availability>();
        CreateMap<Service.Models.ResearchDataset.Contributor, Contributor>();
        CreateMap<Service.Models.ResearchDataset.DatasetRelation, DatasetRelation>();
        CreateMap<Service.Models.ResearchDataset.Organisation, Organisation>();
        CreateMap<Service.Models.ResearchDataset.Person, Person>();
        CreateMap<Service.Models.ResearchDataset.ResearchDataCatalog, ResearchDataCatalog>();
        CreateMap<Service.Models.ResearchDataset.Role, Role>();
        CreateMap<Service.Models.ResearchDataset.Version, Version>();

    }
}