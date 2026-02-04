namespace CSC.PublicApi.Repositories;

public static class RepositoryHelpers
{
    public static string GetURNLink(string URN)
    {
        return $"https://urn.fi/{URN}";
    }
}