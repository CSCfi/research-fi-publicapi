﻿using System.Diagnostics;
using CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;
using Nest;

namespace CSC.PublicApi.ElasticService;

public class ElasticSearchService<TIn, TOut> : ISearchService<TIn, TOut> where TOut : class
{
    private readonly IElasticClient _elasticClient;
    private readonly IQueryGenerator<TIn, TOut> _queryGenerator;

    public ElasticSearchService(
        IElasticClient elasticClient,
        IQueryGenerator<TIn, TOut> queryGenerator)
    {
        _elasticClient = elasticClient;
        _queryGenerator = queryGenerator;
    }

    public async Task<(IEnumerable<TOut>, SearchResult)> Search(TIn parameters, int pageNumber, int pageSize)
    {
        var query = _queryGenerator.GenerateQuery(parameters, pageNumber, pageSize);

        var searchResult = await _elasticClient.SearchAsync(query);

        if (Debugger.IsAttached)
        {
            // Enables seeing the query sent to elastic and the response in the log when debugging.
            Console.WriteLine(searchResult.DebugInformation);            
        }
        
        return (searchResult.Documents, new SearchResult(pageNumber, pageSize, searchResult.HitsMetadata?.Total.Value));
    }

    public async Task<(IEnumerable<TOut>, SearchAfterResult)> SearchAfter(TIn parameters, int pageSize, long? searchAfter)
    {
        var query = _queryGenerator.GenerateQuerySearchAfter(parameters, pageSize, searchAfter);

        var searchResult = await _elasticClient.SearchAsync(query);

        if (Debugger.IsAttached)
        {
            // Enables seeing the query sent to elastic and the response in the log when debugging.
            Console.WriteLine(searchResult.DebugInformation);            
        }
        
        long? searchAfterValue = null;
        
        if (searchResult.Hits.Count > 0) {
            searchAfterValue = (long)searchResult.Hits.Last().Sorts.First();
        }

        // Get total number of hits that match the query.
        // Must use separate count query, since with normal search query Elasticsearch reports max 10000 as total hits.
        // This adds additional load to Elasticsearch, it should only be used with "search after" queries.
        long? totalResults = null;
        var countQuery = _queryGenerator.GenerateCountQuery(parameters);
        try
        {
            var countResult = await _elasticClient.CountAsync(countQuery);
            totalResults = countResult.Count;
        }
        catch (System.Exception)
        {
            // Pass without throwing an exception
        }

        return (searchResult.Documents, new SearchAfterResult(searchAfterValue, pageSize, totalResults));
    }

    public async Task<TOut?> GetSingle(string id)
    {
        var query = _queryGenerator.GenerateSingleQuery(id);
        
        var searchResult = await _elasticClient.SearchAsync(query);
        
        if (Debugger.IsAttached)
        {
            // Enables seeing the query sent to elastic and the response in the log when debugging.
            Console.WriteLine(searchResult.DebugInformation);            
        }
        
        return searchResult.Documents.FirstOrDefault();

    }
}