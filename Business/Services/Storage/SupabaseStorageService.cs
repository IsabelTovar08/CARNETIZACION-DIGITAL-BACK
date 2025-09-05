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

        public async Task<(string PublicUrl, string StoragePath)> UploadAsync(
            Stream content,
            string contentType,
            string destinationPath)
        {
            _client.DefaultRequestHeaders.Add("x-upsert", "true");

            var uri = $"{_opt.PublicBaseUrl}/{_opt.Bucket}/{destinationPath}";
            using var streamContent = new StreamContent(content);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var res = await _client.PutAsync(uri, streamContent);
            if (!res.IsSuccessStatusCode)
                throw new Exception($"Supabase upload failed: {res.StatusCode}");

            var publicUrl = $"{_opt.PublicBaseUrl}/{_opt.Bucket}/{destinationPath}";
            return (publicUrl, destinationPath);
        }

        public async Task DeleteIfExistsAsync(string storagePath)
        {
            if (string.IsNullOrEmpty(storagePath)) return;

            var uri = $"{_opt.PublicBaseUrl}/{_opt.Bucket}/delete";
            var payload = new { prefixes = new[] { storagePath } };

            await _client.PostAsJsonAsync(uri, payload);
        }
    }

}
