using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<(string PublicUrl, string StoragePath)> UpsertAsync(
            IEnumerable<string?> pathParts,
            string? previousStoragePath,
            Stream content,
            string contentType,
            string fileName)
        {
            // (ES): 1) Construir ruta destino: {parts...}/{Guid}{ext}
            var ext = Path.GetExtension(fileName);
            var cleanParts = (pathParts ?? Array.Empty<string?>())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p!.Trim().Trim('/'))
                .ToList();

            cleanParts.Add($"{Guid.NewGuid()}{ext}");

            var path = string.Join("/", cleanParts);

            // (ES): 2) Subir
            var (publicUrl, storagePath) = await _storage.UploadAsync(content, contentType, path);

            // (ES): 3) Eliminar anterior si corresponde
            if (!string.IsNullOrWhiteSpace(previousStoragePath) &&
                !string.Equals(previousStoragePath, storagePath, StringComparison.OrdinalIgnoreCase))
            {
                await _storage.DeleteIfExistsAsync(previousStoragePath);
            }

            return (publicUrl, storagePath);
        }
    }
}
