using CareerHub.Entities.Entities;

namespace CareerHub.Business.Services.Abstract
{
    public interface IElasticsearchService
    {
        Task IndexJobAsync(Job job);
        Task<IEnumerable<Job>> SearchJobsAsync(DateTime? start, DateTime? end);
    }

}
