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
using Utilities.Responses;
using Web.Controllers.Base;

namespace Web.Controllers.ModelSecurity
{
    public class PersonController : GenericController<Person, PersonDtoRequest, PersonDto>
    {
        private readonly IPersonBusiness _personBusiness;
        private readonly ILogger<PersonController> _Ilogger;
        public PersonController(IPersonBusiness personBusiness, ILogger<PersonController> logger) : base(personBusiness, logger)
        {
            _personBusiness = personBusiness;
            _Ilogger = logger;
        }

        /// <summary>
        /// Guarda una persona y su usuario asociado (operación compuesta).
        /// POST api/person/save-person-with-user
        /// </summary>
        [HttpPost("save-person-with-user")]
        [Authorize]
        public async Task<IActionResult> SavePersonAndUser([FromBody] PersonRegistrer person)
         {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? (e.Exception?.Message ?? "Error de validación") : e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Fail("Datos inválidos.", errors));
            }

            try
            {
                var result = await _personBusiness.SavePersonAndUser(person);
                return Ok(ApiResponse<PersonRegistrerDto>.Ok(result, "Persona y usuario creados correctamente."));
            }
            catch (ValidationException vex)
            {
                _Ilogger.LogWarning(vex, "Validación al guardar persona y usuario.");
                return BadRequest(ApiResponse<object>.Fail(vex.Message));
            }
            catch (ExternalServiceException esx)
            {
                _Ilogger.LogError(esx, "Error externo al guardar persona y usuario.");
                return StatusCode(500, ApiResponse<object>.Fail(esx.Message));
            }
            catch (Exception ex)
            {
                _Ilogger.LogError(ex, "Error inesperado al guardar persona y usuario.");
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al procesar la solicitud."));
            }
        }


        /// <summary>
        /// Subir / actualizar foto de la persona
        /// POST api/person/{id:int}/photo
        /// </summary>
        [HttpPost("{id:int}/photo")]
        [Authorize]
        public async Task<IActionResult> UploadPhoto(int id, [FromForm] UploadFile photo)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<object>.Fail("Ingresa un id válido."));

            var file = photo?.file;
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<object>.Fail("El archivo es requerido."));

            try
            {
                await using var stream = file.OpenReadStream();
                var (url, path) = await _personBusiness.UpsertPersonPhotoAsync(
                    id,
                    stream,
                    file.ContentType,
                    file.FileName
                );

                var data = new
                {
                    personId = id,
                    photoUrl = url,
                    photoPath = path
                };

                return Ok(ApiResponse<object>.Ok(data, "Foto actualizada correctamente."));
            }
            catch (KeyNotFoundException knf)
            {
                _Ilogger.LogInformation(knf, "Persona no encontrada al subir foto: {Id}", id);
                return NotFound(ApiResponse<object>.Fail(knf.Message));
            }
            catch (ValidationException vex)
            {
                _Ilogger.LogWarning(vex, "Validación fallida al subir foto para persona {Id}", id);
                return BadRequest(ApiResponse<object>.Fail(vex.Message));
            }
            catch (ExternalServiceException esx)
            {
                _Ilogger.LogError(esx, "Error externo al subir foto para persona {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail(esx.Message));
            }
            catch (Exception ex)
            {
                _Ilogger.LogError(ex, "Error inesperado al subir foto para persona {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al procesar la subida de la foto."));
            }
        }

        /// <summary>
        /// Obtiene la información detallada de persona (info personalizada).
        /// GET api/person/personal-info/{id}
        /// </summary>
        [HttpGet("personal-info/{id:int}")]
        [Authorize] // Ajusta si este endpoint debe ser público
        public async Task<IActionResult> PersonaInfo(int id)
        {
            if (id == null || id == 0)
                return BadRequest(new { status = false, message = "Ingresa un id válido" });

            PersonInfoDto? personalInfo = await _personBusiness.GetPersonInfoAsync(id);

                if (personalInfo == null)
                    return NotFound(ApiResponse<object>.Fail("No se encontró información para la persona solicitada."));

                return Ok(ApiResponse<PersonInfoDto>.Ok(personalInfo, "Información obtenida correctamente."));
            }
            catch (ValidationException vex)
            {
                _Ilogger.LogWarning(vex, "Validación al obtener información personal para {Id}", id);
                return BadRequest(ApiResponse<object>.Fail(vex.Message));
            }
            catch (ExternalServiceException esx)
            {
                _Ilogger.LogError(esx, "Error externo al obtener información personal para {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail(esx.Message));
            }
            catch (Exception ex)
            {
                _Ilogger.LogError(ex, "Error inesperado al obtener información personal para {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al obtener la información."));
            }
        }
        /// <summary>
        /// Devuelve la persona asociada al usuario del token.
        /// GET api/person/me/person
        /// </summary>
        [HttpGet("me/person")]
        [Authorize]
        public async Task<IActionResult> GetMyPerson()
        {
            try
            {
                var dto = await _personBusiness.GetMyPersonAsync();

                if (dto == null)
                {
                    
                    return NotFound(ApiResponse<object>.Fail("No se encontró información de la persona asociada al usuario actual."));
                }

                return Ok(ApiResponse<PersonDto>.Ok(dto, "Información de la persona obtenida correctamente."));
            }
            catch (UnauthorizedAccessException uaex)
            {
                _Ilogger.LogWarning(uaex, "Intento de acceso sin identificar al obtener persona actual.");
                return Unauthorized(ApiResponse<object>.Fail("No se pudo identificar al usuario actual. Verifique el token."));
            }
            catch (KeyNotFoundException knf)
            {
                _Ilogger.LogInformation(knf, "Persona no encontrada para el usuario actual.");
                return NotFound(ApiResponse<object>.Fail(knf.Message));
            }
            catch (ExternalServiceException esex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message, // "No se encontró la persona asociada al usuario actual."
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _Ilogger.LogError(ex, "Error inesperado al obtener información de la persona.");
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al obtener la información de la persona."));
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
