namespace CareerHub.DataAccess.Repositories.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity :class,new()
    {
      
        Task<TEntity> GetByIdAsync(object Id);
        Task<TEntity> AddAsync(TEntity entity); 
        void Update(TEntity entity); 
    }
}
