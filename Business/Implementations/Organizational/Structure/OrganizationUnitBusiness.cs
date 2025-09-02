using AutoMapper;
using Business.Classes.Base;
using Data.Interfases;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using Data.Implementations.Organizational.Structure;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Data.Interfases.Organizational.Structure;
using Business.Interfaces.Organizational.Structure;

namespace Business.Implementations.Organizational.Structure
{
    public class OrganizationalUnitBusiness : BaseBusiness<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto>, IOrganizationUnitBusiness
    {
        private readonly IInternalDivisionData _divisionData;
        private readonly IOrganizationalUnitBranchData _branchData;
        public OrganizationalUnitBusiness(
            ICrudBase<OrganizationalUnit> data,
            ILogger<OrganizationalUnit> logger,
            IMapper mapper,
            IInternalDivisionData divisionData,   
            IOrganizationalUnitBranchData branchData 
        ) : base(data, logger, mapper)
        {
            _divisionData = divisionData;
            _branchData = branchData;
        }

        // Valida lo mínimo necesario para crear/actualizar
        public void Validate(OrganizationalUnitDtoRequest dto)
        {
            var errors = new List<string>();
            if (dto == null) throw new ValidationException("La UO no puede ser nula.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add("La descripción es obligatoria.");

            if (errors.Any())
                throw new ValidationException(string.Join(" | ", errors));
        }



        // Conteo de divisiones
        public Task<int> CountDivisionAsync(int organizationalUnitId) =>
            _divisionData.CountByOrgUnitAsync(organizationalUnitId);

        //Conteo de branch

        public Task<int> CountBranchesAsync(int organizationalUnitId) =>
            _branchData.CountBranchesByOrgUnitAsync(organizationalUnitId);

        public async Task<IReadOnlyList<InternalDivisionDto>> GetInternalDivisionsAsync(int organizationalUnitId, CancellationToken ct = default)
        {

            // Traer divisiones desde Data
            var list = await _divisionData.ListByOrgUnitAsync(organizationalUnitId);

            // Mapear a DTOs
            return _mapper.Map<List<InternalDivisionDto>>(list);


        }
    }
}
