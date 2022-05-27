namespace Api.Services
{
    public interface ISearchService<TIn, TOut> where TOut : class
    {
        Task<IReadOnlyCollection<TOut>> Search(TIn searchParameters);
    }
}
