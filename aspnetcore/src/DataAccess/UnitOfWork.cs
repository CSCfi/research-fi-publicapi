﻿using CSC.PublicApi.DataAccess.Repositories;

namespace CSC.PublicApi.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApiDbContext _context;
    public IFundingCallRepository FundingCalls { get; private set; }

    public UnitOfWork(ApiDbContext context, IFundingCallRepository fundingCallRepository)
    {
        _context = context;
        FundingCalls = fundingCallRepository;
    }


    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}