using CareerHub.Entities.Entities;

namespace CareerHub.DataAccess.Repositories.Abstract
{
    public interface ICompanyRepository:IGenericRepository<Company>
    {
        Task<Company> GetByPhoneNumberAsync(string phoneNumber);
    }
}
