using Microsoft.EntityFrameworkCore;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB.Classes
{
    public class MySqlDbContextFactory : IDbContextFactory
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("MySqlConnection");
            //optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        }
    }
}
