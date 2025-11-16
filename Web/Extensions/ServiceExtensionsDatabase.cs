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
            // 🔹 Obtener el proveedor de base de datos definido en appsettings.json
            // 🔹 Obtener el proveedor de base de datos definido en appsettings.json (ej: "SqlServer", "PostgreSQL")
            var dbProvider = configuration["DatabaseProvider"];

            // 🔹 Buscar la cadena de conexión en este orden:
            // 1️⃣ Variable de entorno (para Docker, Azure, etc.)
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            // 2️⃣ appsettings.json específica del proveedor (ej: "ConnectionStrings:PostgreSQL")
            if (string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(dbProvider))
            {
                connectionString = configuration.GetConnectionString(dbProvider);
            }

            // 3️⃣ appsettings.json por defecto (fallback)
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            // 🔹 Validar que haya encontrado una cadena válida
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("❌ No se encontró una cadena de conexión válida para la base de datos.");

            // 🔹 Log informativo para verificar en Docker logs
            Console.WriteLine($"🔌 Proveedor de base de datos detectado: {dbProvider}");
            Console.WriteLine($"🔗 Cadena de conexión utilizada: {connectionString}");

            // 🔹 Seleccionar la fábrica de contexto según el proveedor
            var factory = DbContextFactorySelector.GetFactory(dbProvider);

            // 🔹 Registrar el DbContext con la configuración correcta
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                factory.Configure(options, connectionString);
            });

            return services;
        }
    }
}