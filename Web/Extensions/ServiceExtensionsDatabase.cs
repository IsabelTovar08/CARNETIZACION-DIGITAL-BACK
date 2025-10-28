using Entity.Context;
using Web.FactoryDB;
using Microsoft.EntityFrameworkCore;

namespace Web.Extensions
{
    /// <summary>
    /// Extensión de servicios para configurar la base de datos.
    /// Detecta automáticamente la cadena de conexión desde variables de entorno
    /// o desde appsettings.json. Además selecciona el proveedor correcto (SQL Server o PostgreSQL)
    /// utilizando el patrón Factory.
    /// </summary>
    public static class ServiceExtensionsDatabase
    {
        /// <summary>
        /// Registra la configuración de conexión a base de datos.
        /// </summary>
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Obtener el proveedor de base de datos definido en appsettings.json
            var dbProvider = configuration["DatabaseProvider"];

            // Obtener la cadena de conexión: primero desde variables de entorno (Docker), luego desde appsettings.json
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                                  ?? configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("No se encontró una cadena de conexión válida para la base de datos.");

            // Log informativo para verificar en Docker logs
            Console.WriteLine($"Base de datos detectada: {dbProvider}");
            Console.WriteLine($"Cadena de conexión usada: {connectionString}");

            // Seleccionar la fábrica de contexto según el proveedor (SQL Server / PostgreSQL)
            var factory = DbContextFactorySelector.GetFactory(dbProvider);

            // Registrar el DbContext con la configuración correcta
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                // Se pasa manualmente la cadena de conexión actualizada
                factory.Configure(options, connectionString);
            });

            return services;
        }
    }
}
