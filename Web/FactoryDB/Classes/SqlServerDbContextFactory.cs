using Microsoft.EntityFrameworkCore;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB.Classes
{
    public class SqlServerDbContextFactory : IDbContextFactory
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
