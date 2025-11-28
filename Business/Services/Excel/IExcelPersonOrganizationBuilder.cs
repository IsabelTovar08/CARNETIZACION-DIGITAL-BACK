using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Request;

namespace Business.Services.Excel
{
    /// <summary>
    /// Define la lógica para enriquecer el DTO de persona con datos organizacionales.
    /// </summary>
    public interface IExcelPersonOrganizationBuilder
    {
        Task<PersonDtoRequest> EnrichPersonWithOrganizationAsync(PersonDtoRequest dto);
    }
}
