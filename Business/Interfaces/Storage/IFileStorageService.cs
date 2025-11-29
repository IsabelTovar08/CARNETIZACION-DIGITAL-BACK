using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfases.Storage
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Upload or replace a file at destinationPath. Returns (publicUrl, storagePath).
        /// </summary>
        Task<(string PublicUrl, string StoragePath)> UploadAsync(Stream content, string contentType, string destinationPath, string bucket);    


        /// <summary>
        /// Idempotent delete if exists.
        /// </summary>
        Task DeleteIfExistsAsync(string storagePath, string bucket);

    }
}
