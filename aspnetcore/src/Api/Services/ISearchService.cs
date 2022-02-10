namespace Api.Services
{
    public interface ISearchService<TIn, TOut> where TOut : class
    {
        IReadOnlyCollection<TOut> Search(TIn searchParameters);
    }
}
