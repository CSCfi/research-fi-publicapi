using Api.DatabaseContext;
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
                // TODO: check if these are needed
                //.Where(fd =>
                //    fd.DimTypeOfFunding.TypeId != "62" &&   // Tutkijatohtorin tutkimuskulut
                //    fd.DimTypeOfFunding.TypeId != "66" &&   // Akatemiatutkijan tutkimuskulut
                //    fd.DimTypeOfFunding.TypeId != "69")     // Akatemiaprofessorin tutkimuskulut
                .Where(fd => fd.Id != -1)
                .ProjectTo<FundingDecision>(_mapper.ConfigurationProvider);
        }
    }
}
