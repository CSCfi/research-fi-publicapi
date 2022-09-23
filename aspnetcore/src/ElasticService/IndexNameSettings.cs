using CSC.PublicApi.Service.Models.FundingCall;

namespace CSC.PublicApi.ElasticService;

/// <summary>
/// Provides settings for ElasticSearch index names. Populated during startup from appsettings.
/// </summary>
public class IndexNameSettings : Dictionary<string, string>
{
    public string GetIndexNameForType(Type type)
    {
        var indexNameFound = TryGetValue(type.FullName!, out var indexName);

        if (!indexNameFound || string.IsNullOrWhiteSpace(indexName))
        {
            throw new InvalidOperationException($"Index name for type '{type.Name}' is not configured.");
        }

        return indexName;
    }

    public Dictionary<string, Type> GetTypesAndIndexNames()
    {
        var serviceAssembly = typeof(FundingCall).Assembly;

        var typeNamesAndIndexNamesFromConfig = this;

        if (typeNamesAndIndexNamesFromConfig?.Any() != true)
        {
            throw new InvalidOperationException("Index name settings not configured.");
        }

        var typesAndIndexNames = typeNamesAndIndexNamesFromConfig
            .Select(typeNameAndIndexName => new
            {
                Type = serviceAssembly.GetType(typeNameAndIndexName.Key) ?? throw new InvalidOperationException($"Could not locate model type from assembly for configured model '{typeNameAndIndexName.Key}'."),
                IndexName = typeNameAndIndexName.Value
            })
            .ToDictionary(pair => pair.IndexName, pair => pair.Type);

        return typesAndIndexNames;
    }
}