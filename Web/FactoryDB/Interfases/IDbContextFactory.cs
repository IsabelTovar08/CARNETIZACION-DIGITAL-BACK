using Microsoft.EntityFrameworkCore;

namespace Web.FactoryDB.Interfases
{
    /// <summary>
    /// Define la interfaz para las fábricas de DbContext.
    /// Permite configurar el proveedor de base de datos (SQL Server o PostgreSQL)
    /// usando directamente la cadena de conexión proveniente del entorno (Docker o appsettings).
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary>
        /// Configura el contexto de base de datos utilizando la cadena de conexión.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de Entity Framework.</param>
        /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
        void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString);
    }
}
