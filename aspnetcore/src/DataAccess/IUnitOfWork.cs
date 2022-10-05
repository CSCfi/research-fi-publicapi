using CSC.PublicApi.DataAccess.Repositories;

namespace CSC.PublicApi.DataAccess;

public interface IUnitOfWork
{
    IFundingCallRepository FundingCalls { get; }
    Task CompleteAsync();
}