using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces.Storage;
using Business.Interfases.Storage;

namespace Business.Services.Storage
{
    public class AssetUploader : IAssetUploader
    {
        private readonly IFileStorageService _storage;

        public AssetUploader(IFileStorageService storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Sube o reemplaza un archivo a Supabase con bucket dinámico.
        /// </summary>
        public async Task<(string PublicUrl, string StoragePath)> UpsertAsync(
            IEnumerable<string?> pathParts,
            string? previousStoragePath,
            Stream content,
            string contentType,
            string fileName,
            string bucket)
        {
            // 1) Armar ruta limpia destino sin bucket
            var cleanParts = (pathParts ?? Array.Empty<string?>())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p!.Trim().Trim('/'))
                .ToList();

            cleanParts.Add(fileName.Trim());

            // templates/4/front/front.svg
            string destinationPath = string.Join("/", cleanParts);

            // 2) Subir a Supabase
            var (publicUrl, storagePath) = await _storage.UploadAsync(
                content,
                contentType,
                destinationPath,
                bucket
            );

            // 3) Eliminar archivo anterior si es distinto
            //if (!string.IsNullOrWhiteSpace(previousStoragePath) &&
            //    !string.Equals(previousStoragePath, storagePath, StringComparison.OrdinalIgnoreCase))
            //{
            //    await _storage.DeleteIfExistsAsync(previousStoragePath, bucket);
            //}

            return (publicUrl, storagePath);
        }
    }
}
