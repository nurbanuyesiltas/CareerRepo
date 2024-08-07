using CareerHub.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.DataAccess.Contexts
{
    public class CareerHubDbContext:DbContext
    {
        public CareerHubDbContext(DbContextOptions<CareerHubDbContext> options)
      : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }

    }
}
