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
    }
}