using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Infrastructure
{
    public class InfrastructureSearchParameters
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
}
