using CSC.PublicApi.ElasticService;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace CSC.PublicApi.Interface.Controllers;

public static class ResponseHelper
{
    private const string Last = "last";
    private const string First = "first";
    private const string Previous = "prev";
    private const string Next = "next";

    /// <summary>
    /// Adds response headers for pagination.
    /// </summary>
    /// <param name="httpContext">Current <see cref="HttpContent"/></param>
    /// <param name="results">The results of the search query.</param>
    public static void AddPaginationResponseHeaders(HttpContext httpContext, SearchResult results)
    {
        httpContext.Response.Headers.Add("x-current-page", results.PageNumber.ToString());
        httpContext.Response.Headers.Add("x-page-count", results.TotalPages.ToString());
        httpContext.Response.Headers.Add("x-page-size", results.PageSize.ToString());
        httpContext.Response.Headers.Add("x-total", results.TotalResults.ToString());
        httpContext.Response.Headers.Add("link", GetLinks(httpContext.Request, results.PageNumber, results.TotalPages));
    }

    public static void AddPaginationResponseHeadersSearchAfter(HttpContext httpContext, SearchAfterResult searchAfterResult)
    {
        httpContext.Response.Headers.Add("x-page-size", searchAfterResult.PageSize.ToString());
        if (searchAfterResult.SearchAfter != null)
        {
            httpContext.Response.Headers.Add("x-next-page-token", searchAfterResult.SearchAfter.ToString());
            httpContext.Response.Headers.Add("link", GetLinksSearchAfter(httpContext.Request, searchAfterResult.SearchAfter));
        }
    public static void AddVirtaPaginationResponseHeaders(HttpContext httpContext, int pageNumber, int pageSize)
    {
        httpContext.Response.Headers.Add("x-current-page", pageNumber.ToString());
        httpContext.Response.Headers.Add("x-page-size", pageSize.ToString());
    }

    private static string GetLinks(HttpRequest httpRequest, int pageNumber, int totalPages)
    {
        var url = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}";
        var queryValues = QueryHelpers.ParseQuery(httpRequest.QueryString.Value);
        var result = string.Empty;
        if (pageNumber < totalPages)
        {
            result += $"{CreateLink(url, queryValues, pageNumber + 1, Next)}, ";
        }
        result += $"{CreateLink(url, queryValues, totalPages, Last)}, {CreateLink(url, queryValues, 1, First)}";
        if (pageNumber > 1)
        {
            result += $", {CreateLink(url, queryValues, pageNumber - 1, Previous)}";
        }
        return result;
    }
    private static string GetLinksSearchAfter(HttpRequest httpRequest, long? searchAfter)
    {
        var url = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}";
        var queryValues = QueryHelpers.ParseQuery(httpRequest.QueryString.Value);
        var result = string.Empty;
        if (searchAfter != null) {
            result += $"{CreateLinkSearchAfter(url, queryValues, searchAfter, Next)}";
        }
        return result;
    }

    private static string CreateLink(string url, Dictionary<string, StringValues> query, int pageNumber, string rel)
    {
        query["pageNumber"] = pageNumber.ToString();
        return $"<{QueryHelpers.AddQueryString(url, query)}>; rel=\"{rel}\"";
    }
    private static string CreateLinkSearchAfter(string url, Dictionary<string, StringValues> query, long? searchAfter, string rel)
    {
        query["NextPageToken"] = searchAfter.ToString();
        return $"<{QueryHelpers.AddQueryString(url, query)}>; rel=\"{rel}\"";
    }
}