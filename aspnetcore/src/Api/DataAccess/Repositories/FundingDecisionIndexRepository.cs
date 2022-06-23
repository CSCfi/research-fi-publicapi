﻿using Api.DatabaseContext;
using Api.Models.Entities;
using Api.Models.FundingDecision;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class FundingDecisionIndexRepository : IndexRepositoryBase<FundingDecision>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public FundingDecisionIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override IQueryable<FundingDecision> GetAll()
        {
            return _context
                .Set<DimFundingDecision>()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(fd => fd.Id != -1)
                .ProjectTo<FundingDecision>(_mapper.ConfigurationProvider);
        }

        public override List<object> PerformInMemoryOperations(List<object> objects)
        {
            objects.ForEach(o =>
            {
                var fd = (FundingDecision)o;
                // For akatemia decisions we move consortia from temporary property to the main one.
                if (fd.OrganizationConsortia?.Any() != true)
                {
                    fd.OrganizationConsortia = fd.OrganizationConsortia2;
                }
                
            });
            return objects;
        }


    }
}
