using Web.FactoryDB.Factories;
using Web.FactoryDB.Interfases;

namespace Web.FactoryDB
{
    public class DbContextFactorySelector
    {
        public static IDbContextFactory GetFactory(string provider)
        {
            return provider switch
            {
                "SqlServer" => new SqlServerFactory(),
                //"MySql" => new MySqlFactory(),
                "Postgres" => new PostgresFactory(),
                _ => throw new NotSupportedException($"proveedor{provider} no soportado"),
            };
        }
    }
}
