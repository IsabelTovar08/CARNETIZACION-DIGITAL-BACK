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

        /// <summary>
        /// Sube un archivo a Supabase Storage.
        /// </summary>
        /// <summary>
        /// Sube o reemplaza un archivo en Supabase Storage usando PUT.
        /// </summary>
        public async Task<(string PublicUrl, string StoragePath)> UploadAsync(
            Stream content,
            string contentType,
            string destinationPath,
            string bucket)
        {
            // ❗ destinationPath NO debe contener el bucket
            // Ejemplo correcto: "5/person_5.jpg"
            var storagePath = destinationPath;

            // Construcción correcta del endpoint
            var requestUrl = $"storage/v1/object/{bucket}/{storagePath}";

            using var request = new HttpRequestMessage(HttpMethod.Put, requestUrl)
            {
                Content = new StreamContent(content)
            };

            // Headers de contenido
            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue(contentType);

            // Headers de autorización
            request.Headers.Add("apikey", _opt.ServiceRoleKey);

            // Ejecución
            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed => {error}");
            }

            // URL pública correcta
            var publicUrl = $"{_opt.PublicBaseUrl}/{bucket}/{storagePath}";

            return (publicUrl, storagePath);
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
