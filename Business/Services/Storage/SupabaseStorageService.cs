using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases.Storage;
using Entity.DTOs.Options;
using Microsoft.Extensions.Options;

namespace Business.Implementations.Storage
{
    public class SupabaseStorageService : IFileStorageService
    {
        private readonly HttpClient _client;
        private readonly SupabaseOptions _opt;

        public SupabaseStorageService(HttpClient client, IOptions<SupabaseOptions> options)
        {
            _client = client;
            _opt = options.Value;
            _client.BaseAddress = new Uri(_opt.Url);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _opt.ServiceRoleKey);
        }

        public async Task<(string PublicUrl, string StoragePath)> UploadAsync(Stream content,string contentType,string destinationPath,string bucket)
        {
            // Limpieza de path
            destinationPath = destinationPath.TrimStart('/');

            // Permitir reemplazo
            if (!_client.DefaultRequestHeaders.Contains("x-upsert"))
                _client.DefaultRequestHeaders.Add("x-upsert", "true");

            var uri = $"{_opt.Url}/storage/v1/object/{bucket}/{destinationPath}";

            using var streamContent = new StreamContent(content);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var res = await _client.PutAsync(uri, streamContent);
            if (!res.IsSuccessStatusCode)
                throw new Exception($"Supabase upload failed: {res.StatusCode}");

            var publicUrl = $"{_opt.PublicBaseUrl}/{bucket}/{destinationPath}";

            return (publicUrl, destinationPath);
        }


        public async Task DeleteIfExistsAsync(string storagePath, string bucket)
        {
            if (string.IsNullOrEmpty(storagePath))
                return;

            storagePath = storagePath.TrimStart('/');

            var endpoint = $"storage/v1/object/{bucket}/delete";

            var payload = new { prefixes = new[] { storagePath } };

            var res = await _client.PostAsJsonAsync(endpoint, payload);

            if (!res.IsSuccessStatusCode)
                throw new Exception($"Supabase delete failed: {res.StatusCode}");
        }

    }

}
