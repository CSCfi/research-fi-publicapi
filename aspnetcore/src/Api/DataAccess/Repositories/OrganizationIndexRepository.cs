using Api.DatabaseContext;
using Api.Models.Organization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class OrganizationIndexRepository : IIndexRepository<Organization>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAsyncEnumerable<Organization> GetAllAsync()
        {
            return _context.DimOrganizations
                .Where(organization => organization.Id != -1)
                .ProjectTo<Organization>(_mapper.ConfigurationProvider)
                .AsAsyncEnumerable();
        }
    }
}
