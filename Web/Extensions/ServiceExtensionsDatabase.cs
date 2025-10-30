using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.FactoryDB;

namespace Web.Config
{
    /// <summary>
    /// Clase de extensión para configurar la base de datos según el ambiente y proveedor.
    /// Lee el ambiente desde el archivo .env en la raíz del proyecto
    /// y obtiene la cadena de conexión desde devops/{ENVIRONMENT}/.env.
    /// </summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Configura el DbContext de la aplicación de acuerdo al proveedor y ambiente definidos.
        /// </summary>
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Obtener el proveedor base (fallback)
            var dbProvider = configuration["DatabaseProvider"];

            // 2. Determinar la raíz de la solución (un nivel arriba del proyecto Web)
            var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName
                               ?? throw new InvalidOperationException("No se pudo determinar la ruta raíz del proyecto.");

            // 3. Leer el ambiente desde el archivo .env raíz
            var rootEnvPath = Path.Combine(solutionRoot, ".env");
            string? environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(environment) && File.Exists(rootEnvPath))
            {
                var envLine = File.ReadAllLines(rootEnvPath)
                    .FirstOrDefault(l => l.StartsWith("ENVIRONMENT=", StringComparison.OrdinalIgnoreCase));

                if (envLine != null)
                    environment = envLine.Split('=')[1].Trim();
            }

            if (string.IsNullOrWhiteSpace(environment))
                throw new InvalidOperationException("No se pudo determinar el ambiente actual (ENVIRONMENT).");

            // 4. Buscar el archivo .env dentro de devops/{ENVIRONMENT}/.env (en la raíz)
            var envFilePath = Path.Combine(solutionRoot, "devops", environment, ".env");
            string? connectionString = null;

            if (File.Exists(envFilePath))
            {
                var lines = File.ReadAllLines(envFilePath);

                // Cargar todas las variables del archivo al entorno
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains('='))
                    {
                        var parts = line.Split('=', 2);
                        Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
                    }
                }

                // Buscar la cadena de conexión
                var connectionLine = lines.FirstOrDefault(l =>
                    l.StartsWith("ConnectionStrings__DefaultConnection=", StringComparison.OrdinalIgnoreCase));

                if (connectionLine != null)
                {
                    connectionString = connectionLine.Split('=', 2)[1].Trim();

                    // 🔸 Expansión manual de ${VAR} (Docker/Unix style)
                    connectionString = Regex.Replace(connectionString, @"\$\{([^}]+)\}", match =>
                    {
                        var varName = match.Groups[1].Value;
                        var value = Environment.GetEnvironmentVariable(varName);
                        return value ?? match.Value; // si no existe, deja el placeholder
                    });
                }

                // Buscar proveedor (si está definido)
                var providerLine = lines.FirstOrDefault(l =>
                    l.StartsWith("DatabaseProvider=", StringComparison.OrdinalIgnoreCase));

                if (providerLine != null)
                    dbProvider = providerLine.Split('=')[1].Trim();
            }

            // 5. Fallback: usar variables de entorno o appsettings.json
            connectionString ??= Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                              ?? configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("No se encontró una cadena de conexión válida para la base de datos.");

            if (string.IsNullOrWhiteSpace(dbProvider))
                throw new InvalidOperationException("No se especificó el proveedor de base de datos (DatabaseProvider).");

            // 6. Logs para depuración
            Console.WriteLine($"🌎 Ambiente detectado: {environment}");
            Console.WriteLine($"🧩 Proveedor detectado: {dbProvider}");
            Console.WriteLine($"🔗 Cadena de conexión usada: {connectionString}");

            // 7. Crear la fábrica del DbContext
            var factory = DbContextFactorySelector.GetFactory(dbProvider);

            // 8. Registrar el DbContext en el contenedor de servicios
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                factory.Configure(options, connectionString);
            });

            return services;
        }
    }
}
