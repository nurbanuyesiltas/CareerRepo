using CareerHub.Core.Common.Concrete;
using CareerHub.Core.Parameters;
using CareerHub.Entities.Entities;

namespace CareerHub.Business.Services.Abstract
{
    public interface IJobService
    {
        Task<ApiResult<JobRequestModel>> AddJobAsync(JobRequestModel jobRequestModel);
        Task<ApiResult<IEnumerable<Job>>> SearchJobsByPublicationDurationAsync(JobSearchModel searchModel);

    }
}
