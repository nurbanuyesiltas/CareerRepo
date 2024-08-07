using CareerHub.DataAccess.Contexts;
using CareerHub.DataAccess.Repositories.Abstract;

namespace CareerHub.DataAccess.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CareerHubDbContext _dbContext;
        public UnitOfWork(CareerHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
