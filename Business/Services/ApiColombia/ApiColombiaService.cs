using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Interfaces.ApiColombia;
using Entity.DTOs.Organizational.Location;

namespace Business.Services.ApiColombia
{
    public class ApiColombiaService : IColombiaApiService
    {
        private HttpClient _httpClient;
        private const string UrlBase = "https://api-colombia.com/api/v1";

        public ApiColombiaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            var response = await _httpClient.GetAsync($"{UrlBase}/Department");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var departamentos = JsonSerializer.Deserialize<List<DepartmentDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return departamentos!;
        }

        public async Task<List<CityDto>> GetCityesByDepartmentsAsync(int deparmentId)
        {
            var response = await _httpClient.GetAsync($"{UrlBase}/Department/{deparmentId}/cities");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var departamentos = JsonSerializer.Deserialize<List<CityDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return departamentos!;
        }

        
    }
}
