using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Organizational.Response.Location;

namespace Business.Interfaces.ApiColombia
{
    public interface IColombiaApiService
    {
        Task<List<DepartmentDto>> GetDepartmentsAsync();
        Task<List<CityDto>> GetCityesByDepartmentsAsync(int deparmentId);
    }
}

