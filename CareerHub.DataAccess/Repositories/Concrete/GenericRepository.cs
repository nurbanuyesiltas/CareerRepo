using CareerHub.DataAccess.Contexts;
using CareerHub.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.DataAccess.Repositories.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly CareerHubDbContext _dbContext;
        public GenericRepository(CareerHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }
        public virtual async Task<TEntity> GetByIdAsync(object Id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(Id);
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}
