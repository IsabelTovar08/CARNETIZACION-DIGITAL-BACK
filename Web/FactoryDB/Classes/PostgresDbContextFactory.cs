using Microsoft.EntityFrameworkCore;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB.Classes
{
    public class PostgresDbContextFactory : IDbContextFactory
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("⚠️ Connection string 'Postgres' no encontrada en appsettings.json.");

            optionsBuilder.UseNpgsql(connectionString);
        }

    }
}
