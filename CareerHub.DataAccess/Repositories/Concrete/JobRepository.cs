using CareerHub.DataAccess.Contexts;
using CareerHub.DataAccess.Repositories.Abstract;
using CareerHub.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.DataAccess.Repositories.Concrete
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        CareerHubDbContext _dbContext;
        public JobRepository(CareerHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;   
        }
        public async Task<IEnumerable<Job>> SearchJobsByPublicationDurationAsync(DateTime? start, DateTime? end)
        {
            var query = _dbContext.Jobs.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(j => j.PublicationDuration >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(j => j.PublicationDuration <= end.Value);
            }

            return await query.ToListAsync();
        }
    }
}
