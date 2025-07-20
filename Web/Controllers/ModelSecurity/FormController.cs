using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;


namespace Web.Controllers.ModelSecurity
{
    public class FormController : GenericController<Form, FormDto>
    {
        public FormController(IBaseBusiness<Form, FormDto> business, ILogger<FormController> logger) : base(business, logger)
        {
        }
    }
}

