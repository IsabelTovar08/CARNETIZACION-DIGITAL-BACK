using Microsoft.EntityFrameworkCore;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB.Factories
{
    /// <summary>
    /// Fábrica para configurar Entity Framework con PostgreSQL.
    /// </summary>
    public class PostgresFactory : IDbContextFactory
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
