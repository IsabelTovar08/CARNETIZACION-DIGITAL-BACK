using Entity.Context;
using Web.FactoryDB;

namespace Web.Extensions
{
    public static class ServiceExtensionsDatabase
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var dbProvider = configuration["DatabaseProvider"];
            var factory = DbContextFactorySelector.GetFactory(dbProvider);

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                factory.Configure(options, configuration);
            });

            return services;
        }
    }
}
