using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetResearchDatasetsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSv, nameEn sisältää tekstin.
    /// </summary>
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "isLatestVersion")] 
    public bool? IsLatestVersion { get; set; }
}