namespace CareerHub.DataAccess.Repositories.Abstract
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
