﻿using CSC.PublicApi.DatabaseContext;

namespace CSC.PublicApi.Repositories;

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