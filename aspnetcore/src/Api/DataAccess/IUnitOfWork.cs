namespace Api.DataAccess
{
    public interface IUnitOfWork
    {
        IFundingCallRepository FundingCalls { get; }
        Task CompleteAsync();
    }
}
