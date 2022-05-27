using Api.DataAccess.Repositories;

namespace Api.DataAccess
{
    public interface IUnitOfWork
    {
        IFundingCallRepository FundingCalls { get; }
        Task CompleteAsync();
    }
}
