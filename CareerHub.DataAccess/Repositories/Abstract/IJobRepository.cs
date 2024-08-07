using CareerHub.Entities.Entities;

namespace CareerHub.DataAccess.Repositories.Abstract
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<IEnumerable<Job>> SearchJobsByPublicationDurationAsync(DateTime? start, DateTime? end);

    }
}
