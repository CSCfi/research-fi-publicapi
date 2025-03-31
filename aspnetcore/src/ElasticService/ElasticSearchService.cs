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

    public async Task<(IEnumerable<TOut>, SearchAfterResult)> SearchAfter(TIn parameters, int pageSize, string? searchAfter)
    {
        var query = _queryGenerator.GenerateQuerySearchAfter(parameters, pageSize, searchAfter);

        var searchResult = await _elasticClient.SearchAsync(query);

        if (Debugger.IsAttached)
        {
            // Enables seeing the query sent to elastic and the response in the log when debugging.
            Console.WriteLine(searchResult.DebugInformation);            
        }
        
        string? searchAfterValue = null;
        
        if (searchResult.Hits.Count > 0) {
            searchAfterValue = (string)searchResult.Hits.Last().Sorts.First();
        }

        return (searchResult.Documents, new SearchAfterResult(searchAfterValue, pageSize));
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