using Microsoft.EntityFrameworkCore;

namespace Web.FactoryDB.Interfases
{
    public interface IDbContextFactory
    {
        void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration);
    }
}
    