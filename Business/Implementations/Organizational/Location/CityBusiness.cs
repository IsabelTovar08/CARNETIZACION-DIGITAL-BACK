using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfases.Organizational.Location;
using Data.Interfases;
using Data.Interfases.Organizational.Location;
using Entity.DTOs.Organizational.Location.Response;
using Entity.Models.Organizational.Location;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Organizational.Location
{
    public class CityBusiness : BaseBusiness<City, CityDtoResponse, CityDtoResponse>, ICityBusiness
    {
        protected readonly ICityData _cityData;
        public CityBusiness(ICityData cityData, ILogger<City> logger, IMapper mapper) : base(cityData, logger, mapper)
        {
            _cityData = cityData;
        }

        public async Task<List<CityDtoResponse>> GetCityesByDepartmentsAsync(int deparmentId)
        {
            var list = await _cityData.GetCityesByDepartmentsAsync(deparmentId);
            return _mapper.Map<List<CityDtoResponse>>(list);
        }
    }
}
