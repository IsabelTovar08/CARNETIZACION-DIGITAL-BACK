using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.Models;
using Entity.Models.ModelSecurity;
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
