using Business.Classes;
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
        public PersonController(IBaseBusiness<Person,PersonDtoRequest, PersonDto> business, ILogger<PersonController> logger) : base(business, logger)
        {
        }
    }
}