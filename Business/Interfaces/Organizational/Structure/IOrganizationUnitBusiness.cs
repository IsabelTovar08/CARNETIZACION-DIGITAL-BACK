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
    public interface IOrganizationUnitBusiness : IBaseBusiness<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto>
    {
        Task<IReadOnlyList<InternalDivisionDto>> GetInternalDivisionsAsync(int organizationalUnitId, CancellationToken ct = default);
        Task<int> CountDivisionAsync(int organizationalUnitId);
        Task<int> CountBranchesAsync(int organizationalUnitId);
        void Validate(OrganizationalUnitDtoRequest dto);

        //LLamado a las Divisiones internas que tiene La unidad organizativa
        
    }
}
