using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;


namespace Web.Controllers.ModelSecurity
{
    public class FormController : GenericController<Form, FormDtoRequest, FormDto>
    {
        public FormController(IBaseBusiness<Form, FormDtoRequest, FormDto> business, ILogger<FormController> logger) : base(business, logger)
        {
        }
    }
}

