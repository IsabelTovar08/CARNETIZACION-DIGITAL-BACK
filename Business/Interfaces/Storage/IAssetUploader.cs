using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Storage
{
    /// <summary>
    /// Servicio genérico para subir/actualizar archivos en el storage
    /// construyendo rutas limpias y eliminando el anterior si cambia.
    /// </summary>
    public interface IAssetUploader
    {
        /// <summary>
        /// Sube/actualiza un archivo y elimina el previo si era distinto.
        /// pathParts: segmentos de carpeta en orden (se ignoran nulos/vacíos).
        /// Devuelve (PublicUrl, StoragePath).
        /// </summary>
        Task<(string PublicUrl, string StoragePath)> UpsertAsync(
            IEnumerable<string?> pathParts,
            string? previousStoragePath,
            Stream content,
            string contentType,
            string fileName // se usa SOLO para extraer la extensión
        );
    }
}
