using Business.Interfaces.ApiColombia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Web.Controllers.Ubication
{

    [ApiController]
    [Route("api/[controller]")]
    public class UbicacionController : ControllerBase
    {
        private readonly IColombiaApiService _colombiaApiService;

        public UbicacionController(IColombiaApiService colombiaApiService)
        {
            _colombiaApiService = colombiaApiService;
        }

        [HttpGet("departamentos")]
        public async Task<IActionResult> GetDeparments()
        {
            var result = await _colombiaApiService.GetDepartmentsAsync();
            return Ok(result);
        }


        [HttpGet("departamentos/{deparmentId}/municipios")]
        public async Task<IActionResult> GetCytiesByDeparments(int deparmentId)
        {
            var result = await _colombiaApiService.GetDepartmentsAsync();
            return Ok(result);
        }
    }
}
