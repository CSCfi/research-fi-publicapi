using AutoMapper;
using Api.Models.Entities;

namespace Api.Maps
{
    public class ResearchDatasetProfile : Profile
    {
        public ResearchDatasetProfile()
        {
            CreateProjection<DimResearchDataset, Api.Models.ResearchDataset.ResearchDataset>();
        }
    }
}
