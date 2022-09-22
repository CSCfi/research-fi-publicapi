using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Organization
{
    public class OrganizationSearchParameters
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
}
