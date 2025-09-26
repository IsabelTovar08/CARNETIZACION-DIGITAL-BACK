using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Organizational.Location.Response;
using Entity.Models.Organizational.Location;

namespace Business.Interfases.Organizational.Location
{
    public interface ICityBusiness : IBaseBusiness<City, CityDtoResponse, CityDtoResponse>
    {
        Task<List<CityDtoResponse>> GetCityesByDepartmentsAsync(int deparmentId);
    }
}
