namespace CSC.PublicApi.ElasticService;

public class SearchAfterResult
{
    public long? SearchAfter { get; }
    public SearchAfterResult(long? searchAfter)
    {
        SearchAfter = searchAfter;
    }
}