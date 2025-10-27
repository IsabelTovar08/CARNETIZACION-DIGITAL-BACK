using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Organizational.Structure
{
    public  interface IContactOrganizationBusiness : IBaseBusiness<ContactOrganization,ContactOrganizationDtoRequest,ContactOrganizationDtoResponse>
    {
        Task<ContactOrganizationDtoResponse> CreateContactRequest(ContactOrganizationDtoRequest dto);
        Task ApproveContactAsync(int id, bool approved);

    }
}
