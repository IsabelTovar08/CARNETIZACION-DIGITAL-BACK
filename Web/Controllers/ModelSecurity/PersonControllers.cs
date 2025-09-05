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
        public async Task<IActionResult> UploadPhoto(int id, [FromForm] Photo photo)
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
    }

    public class Photo
    {
        public IFormFile file { get; set; }
    }
}