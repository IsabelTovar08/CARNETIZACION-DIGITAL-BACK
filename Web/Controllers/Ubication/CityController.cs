using Business.Interfases;
using Business.Interfases.Organizational.Location;
using Entity.DTOs.Organizational.Location.Response;
using Entity.Models.Organizational.Location;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Ubication
{
    public class CityController : GenericController<City, CityDtoResponse, CityDtoResponse>
    {
        protected readonly ICityBusiness _cityBusiness;
        public CityController(ICityBusiness cityBusiness, ILogger<CityController> logger) : base(cityBusiness, logger)
        {
            _cityBusiness = cityBusiness;
        }

        [HttpGet("city-by-deparment/{id}")]
        public async Task<IActionResult> GetCityByDeparmnet(int id)
        {
            var result = await _cityBusiness.GetCityesByDepartmentsAsync(id);
            return Ok(result);
        }
    }
}
