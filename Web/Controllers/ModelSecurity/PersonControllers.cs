using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.DTOs.Create;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;


namespace Web.Controllers.ModelSecurity
{
    public class PersonController : GenericController<Person, PersonDto>
    {
        public PersonController(IBaseBusiness<Person, PersonDto> business, ILogger<PersonController> logger) : base(business, logger)
        {
        }
    }
}
