using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence.Configuration
{
    public class EnterpriseDBContextFactory : IDesignTimeDbContextFactory<EnterpriseDBContext>
    {
        public EnterpriseDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EnterpriseDBContext>();

            optionsBuilder.UseSqlServer(
                "Server=.;Database=EnterpriseDB;Trusted_Connection=True;TrustServerCertificate=True");

            return new EnterpriseDBContext(optionsBuilder.Options);
        }
    }
}