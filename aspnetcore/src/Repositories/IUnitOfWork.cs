namespace CSC.PublicApi.Repositories;

public interface IUnitOfWork
{
    IFundingCallRepository FundingCalls { get; }
    Task CompleteAsync();
}