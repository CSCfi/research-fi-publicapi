using Api.DatabaseContext;
using Api.Models.Organization;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Api.DataAccess.Repositories
{
    public class OrganizationIndexRepository : IndexRepositoryBase<Organization>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override IQueryable<Organization> GetAll()
        {
            return _context.DimOrganizations
                .Where(organization => organization.Id != -1)
                .ProjectTo<Organization>(_mapper.ConfigurationProvider);
        }
    }
}
