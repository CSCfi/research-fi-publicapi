using Microsoft.AspNetCore.Mvc;

namespace Api.Models.ResearchDataset
{
    public class ResearchDatasetSearchParameters
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
}
