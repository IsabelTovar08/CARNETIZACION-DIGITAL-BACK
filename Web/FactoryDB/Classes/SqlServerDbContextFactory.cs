using Microsoft.EntityFrameworkCore;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB.Factories
{
    /// <summary>
    /// Fábrica para configurar Entity Framework con SQL Server.
    /// </summary>
    public class SqlServerFactory : IDbContextFactory
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
