using Web.FactoryDB.Classes;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB
{
    public class DbContextFactorySelector
    {
        public static IDbContextFactory GetFactory(string provider)
        {
            return provider switch
            {
                "SqlServer" => new SqlServerDbContextFactory(),
                "MySql" => new MySqlDbContextFactory(),
                "Postgres" => new PostgresDbContextFactory(),
                _ => throw new NotSupportedException($"proveedor{provider} no soportado"),
            };
        }
    }
}
