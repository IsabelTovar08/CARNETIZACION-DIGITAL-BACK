using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Organizational.Location.Response;

namespace Business.Interfaces.ApiColombia
{
    public interface IColombiaApiService
    {
        Task<List<DepartmentDtoResponse>> GetDepartmentsAsync();
        Task<List<CityDtoResponse>> GetCityesByDepartmentsAsync(int deparmentId);
    }
}

