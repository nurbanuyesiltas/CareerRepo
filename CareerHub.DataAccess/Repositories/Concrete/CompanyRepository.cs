using CareerHub.DataAccess.Contexts;
using CareerHub.DataAccess.Repositories.Abstract;
using CareerHub.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.DataAccess.Repositories.Concrete
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        CareerHubDbContext _dbContext;
        public CompanyRepository(CareerHubDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;  
        }

        public async Task<Company> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbContext.Companies.FirstOrDefaultAsync(x=>x.PhoneNumber==phoneNumber);

        }
    }
}
