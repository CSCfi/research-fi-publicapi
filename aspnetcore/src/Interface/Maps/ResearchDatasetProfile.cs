using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.ResearchDataset;
using Version = CSC.PublicApi.Interface.Models.ResearchDataset.Version;

namespace CSC.PublicApi.Interface.Maps;

public class ResearchDatasetProfile : Profile
{
    private const string FairDataBaseUrl = "https://etsin.fairdata.fi/dataset/";
    
    public ResearchDatasetProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateMap<GetResearchDatasetsQueryParameters, ResearchDatasetSearchParameters>()
            .ForMember(dst => dst.Language,
                opt => opt.MapFrom(src =>
                    src.Language != null 
                        ? src.Language.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) 
                        : null))
            .ForMember(dst => dst.FieldOfScience,
                opt => opt.MapFrom(src =>
                    src.FieldOfScience != null
                        ? src.FieldOfScience.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        : null));
        
        CreateMap<Service.Models.ResearchDataset.ResearchDataset, ResearchDataset>()
            .ForMember(dst => dst.FairDataUrl, opt => opt.MapFrom(src => $"{FairDataBaseUrl}{src.Identifier}"));
        
        CreateMap<Service.Models.ResearchDataset.PreferredIdentifier, PreferredIdentifier>();
        CreateMap<Service.Models.ResearchDataset.Contributor, Contributor>();
        CreateMap<Service.Models.ResearchDataset.DatasetRelation, DatasetRelation>();
        CreateMap<Service.Models.ResearchDataset.Organisation, Organisation>();
        CreateMap<Service.Models.ResearchDataset.Person, Person>();
        CreateMap<Service.Models.ResearchDataset.ResearchDataCatalog, ResearchDataCatalog>();
        CreateMap<Service.Models.ResearchDataset.Version, Version>();
        CreateMap<Service.Models.ReferenceData, ReferenceData>();
        CreateMap<Service.Models.Keyword, Keyword>();
    }
}