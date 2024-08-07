using CareerHub.Core.Common.Concrete;
using CareerHub.Core.Parameters;

namespace CareerHub.Business.Services.Abstract
{
    public interface ICompanyService
    {
        Task<ApiResult<CompanyRequestModel>> AddCompanyAsync(CompanyRequestModel companyRequestModel);
    }
}
