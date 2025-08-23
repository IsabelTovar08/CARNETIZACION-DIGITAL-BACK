using Microsoft.EntityFrameworkCore;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB.Classes
{
    public class PostgresDbContextFactory : IDbContextFactory
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
           optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
        }
    }
}
