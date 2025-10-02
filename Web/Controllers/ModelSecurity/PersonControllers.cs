using Business.Classes;
using Business.Interfaces.Security;
using Business.Interfases;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;

namespace Web.Controllers.ModelSecurity
{
    public class PersonController : GenericController<Person, PersonDtoRequest, PersonDto>
    {
        protected readonly IPersonBusiness _personBusiness;
        public PersonController(IPersonBusiness personBusiness, ILogger<PersonController> logger) : base(personBusiness, logger)
        {
            _personBusiness = personBusiness;
        }

        [HttpPost("save-person-with-user")]
        public async Task<IActionResult> SavePersonAndUser([FromBody] PersonRegistrer person)
        {
            var result = await _personBusiness.SavePersonAndUser(person);
            return Ok(result);
        }

        [HttpPost("{id:int}/photo")]
        public async Task<IActionResult> UploadPhoto(int id, [FromForm] UploadFile photo)
        {
            var file = photo.file;
            if (file == null || file.Length == 0)
                return BadRequest(new { status = false, message = "File is required" });

            await using var stream = file.OpenReadStream();

            (string url, string path) = await _personBusiness.UpsertPersonPhotoAsync(
                id,
                stream,
                file.ContentType,
                file.FileName);

            return Ok(new
            {
                status = true,
                message = "Photo updated",
                data = new { personId = id, photoUrl = url, photoPath = path }
            });
        }

        [HttpGet("personal-info/{id:int}")]
        public async Task<IActionResult> PersonaInfo(int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { status = false, message = "Ingresa un id válido" });

            PersonInfoDto? personalInfo = await _personBusiness.GetPersonInfoAsync(id);

            return Ok(new
            {
                status = true,
                message = "Información obtenida",
                data = personalInfo
            });
        }

        [HttpGet("me/person")]
        [Authorize]
        public async Task<IActionResult> GetMyPerson()
        {
            try
            {
                var dto = await _personBusiness.GetMyPersonAsync();

                if (dto == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No se encontró información de la persona asociada al usuario actual.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Información de la persona obtenida correctamente.",
                    data = dto
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "No se pudo identificar al usuario actual. Verifique el token.",
                    data = (object?)null
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener información de la persona.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ocurrió un error interno al obtener la información de la persona.",
                    data = (object?)null
                });
            }
        }

        // 🔹 NUEVO ENDPOINT: búsqueda con filtros y paginación
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? internalDivisionId,
            [FromQuery] int? organizationalUnitId,
            [FromQuery] int? profileId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var (items, total) = await _personBusiness.QueryWithFiltersAsync(
                internalDivisionId, organizationalUnitId, profileId, page, pageSize, ct);

            return Ok(new
            {
                success = true,
                message = total > 0 ? "Personas obtenidas correctamente." : "No se encontraron personas con los filtros aplicados.",
                data = items,
                total,
                page,
                pageSize
            });
        }
    }

    public class UploadFile
    {
        public IFormFile file { get; set; }
    }
}
