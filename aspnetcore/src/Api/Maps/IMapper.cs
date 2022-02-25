namespace Api.Maps
{
    public interface IMapper<TIn, TOut> where TIn : class where TOut : class
    {
        TOut Map(TIn input);
    }
}
