using CareerHub.DataAccess.Contexts;
using CareerHub.DataAccess.Repositories.Abstract;
using CareerHub.DataAccess.Repositories.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CareerHub.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// IServiceCollection referanslarının eklenmesini sağlar.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataAccessRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            //DbContext Settings
            services.AddDbContext<CareerHubDbContext>((sp, opt) =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("MssqlConnectionString"));
            });

            //DataAccess Layer Dependencies will be custom added here
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
    
            return services;
        }
    }
}
