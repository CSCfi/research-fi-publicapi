using System;
using System.Collections.Generic;
using System.Linq;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class ResponseHelperContractTest
{
    [Theory]
    [InlineData(1, "next,last,first")]
    [InlineData(2, "next,last,first,prev")]
    [InlineData(3, "last,first,prev")]
    public void Pagination_PreservesFiltersAndBuildsExactRelations(int page, string relations)
    {
        var context = Context("?name=synthetic%20science&category=A%26B&category=C&pageNumber=99&pageSize=10");

        ResponseHelper.AddPaginationResponseHeaders(context, new SearchResult(page, 10, 25));

        Assert.Equal(page.ToString(), context.Response.Headers["x-current-page"]);
        Assert.Equal("3", context.Response.Headers["x-page-count"]);
        Assert.Equal("10", context.Response.Headers["x-page-size"]);
        Assert.Equal("25", context.Response.Headers["x-total"]);
        var links = Links(context);
        Assert.Equal(relations.Split(','), links.Keys);
        foreach (var (relation, uri) in links)
        {
            Assert.Equal("https", uri.Scheme);
            Assert.Equal("api.test", uri.Host);
            Assert.Equal("/v1/funding-calls", uri.AbsolutePath);
            var query = QueryHelpers.ParseQuery(uri.Query);
            Assert.Equal("synthetic science", query["name"]);
            Assert.Equal(new[] { "A&B", "C" }, query["category"].ToArray());
            Assert.Equal("10", query["pageSize"]);
            Assert.Single(query["pageNumber"]);
            var expected = relation switch { "first" => 1, "last" => 3, "next" => page + 1, _ => page - 1 };
            Assert.Equal(expected.ToString(), query["pageNumber"]);
        }
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(9007199254740993L)]
    public void Export_ReplacesCursorAndPreservesFilters(long cursor)
    {
        var context = Context("?name=synthetic%20science&NextPageToken=17&pageSize=2");

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(context, new SearchAfterResult(cursor, 2, 25001));

        Assert.Equal(cursor.ToString(), context.Response.Headers["x-next-page-token"]);
        Assert.Equal("25001", context.Response.Headers["x-total"]);
        Assert.Equal("2", context.Response.Headers["x-page-size"]);
        var link = Assert.Single(Links(context));
        Assert.Equal("next", link.Key);
        var query = QueryHelpers.ParseQuery(link.Value.Query);
        Assert.Equal("synthetic science", query["name"]);
        Assert.Single(query["NextPageToken"]);
        Assert.Equal(cursor.ToString(), query["NextPageToken"]);
        Assert.Equal("2", query["pageSize"]);
    }

    [Fact]
    public void Export_FinalPageHasNoCursorOrLink()
    {
        var context = Context("");

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(context, new SearchAfterResult(null, 2, 25));

        Assert.Equal("2", context.Response.Headers["x-page-size"]);
        Assert.False(context.Response.Headers.ContainsKey("x-next-page-token"));
        Assert.False(context.Response.Headers.ContainsKey("link"));
        Assert.False(context.Response.Headers.ContainsKey("x-total"));
    }

    [Fact]
    public void Pagination_EmptyResultsCurrentlyLinkToPageZero()
    {
        // P4-LINK: characterization, not a valid destination contract.
        var context = Context("?name=synthetic");

        ResponseHelper.AddPaginationResponseHeaders(context, new SearchResult(1, 10, 0));

        Assert.Equal("0", context.Response.Headers["x-page-count"]);
        Assert.Equal("0", context.Response.Headers["x-total"]);
        var links = Links(context);
        Assert.Equal(new[] { "last", "first" }, links.Keys);
        Assert.Equal("0", QueryHelpers.ParseQuery(links["last"].Query)["pageNumber"]);
        Assert.Equal("1", QueryHelpers.ParseQuery(links["first"].Query)["pageNumber"]);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(3, 100)]
    public void VirtaPagination_EmitsOnlyCurrentPageAndSize(int page, int size)
    {
        var context = Context("");

        ResponseHelper.AddVirtaPaginationResponseHeaders(context, page, size);

        Assert.Equal(page.ToString(), context.Response.Headers["x-current-page"]);
        Assert.Equal(size.ToString(), context.Response.Headers["x-page-size"]);
        Assert.Equal(2, context.Response.Headers.Count);
    }

    private static DefaultHttpContext Context(string query)
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("api.test");
        context.Request.Path = "/v1/funding-calls";
        context.Request.QueryString = new QueryString(query);
        return context;
    }

    private static Dictionary<string, Uri> Links(HttpContext context) =>
        context.Response.Headers["link"].ToString().Split(", ", StringSplitOptions.RemoveEmptyEntries)
            .Select(link => link.Split("; rel=\"", StringSplitOptions.None))
            .ToDictionary(parts => parts[1].TrimEnd('"'), parts => new Uri(parts[0].Trim('<', '>')));
}
