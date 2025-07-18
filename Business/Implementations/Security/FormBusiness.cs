using AutoMapper;
using Business.Classes.Base;
using Data.Interfases;
using Entity.DTOs;
using Entity.DTOs.Create;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class FormBusiness :  BaseBusiness<Form, FormDto>
    {
        public FormBusiness(ICrudBase<Form> data, ILogger<Form> logger, IMapper mapper) : base(data, logger, mapper)
        {

        }

        protected void Validate(FormDto form)
        {
            if (form == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(form.Name))
                throw new ValidationException("El Nombre del formulario es obligatorio.");

        }
    }

}
