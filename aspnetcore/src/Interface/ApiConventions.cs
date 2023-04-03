using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface;

public static class ApiConventions
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
    public static void Find([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)] int id)
    {
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
    public static void Search([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)] PaginationQueryParameters id)
    {
    }
    
}