namespace CSC.PublicApi.Repositories;

public static class MemoryCacheKeys
{
    public static string FundingCallByAbbreviationAndEuCallId(string abbreviation, string euCallId)
    {
        return $"fc-sid-{abbreviation}-{euCallId}";
    }
    
    public static string FundingCallBySourceId(string sourceId)
    {
        return $"fc-{sourceId}";
    }

    public static string OrganizationById(int id)
    {
        return $"org-{id}";
    }
}