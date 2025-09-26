using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;

namespace Business.Interfaces.Organizational.Structure
{
    public interface IOrganizationBusiness : IBaseBusiness<Organization, OrganizationDtoRequest, OrganizationDto>
    {
    }
}
