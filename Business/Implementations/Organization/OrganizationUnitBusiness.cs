using AutoMapper;
using Business.Classes.Base;
using Data.Interfases;
using Data.Implementations.Organization;    
using Entity.DTOs.Organizational.Request.Structure;
using Entity.DTOs.Organizational.Response.Structure;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Implementations.Organization
{
    public class OrganizationalUnitBusiness
        : BaseBusiness<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto>
    {
        private readonly InternalDivisionData _divisionData;
        private readonly OrganizationalUnitBranchData _branchData;
        public OrganizationalUnitBusiness(
            ICrudBase<OrganizationalUnit> data,
            ILogger<OrganizationalUnit> logger,
            IMapper mapper,
            InternalDivisionData divisionData,
            OrganizationalUnitBranchData branchData
        ) : base(data, logger, mapper)
        {
            _divisionData = divisionData;
            _branchData = branchData;
        }

        // Valida lo mínimo necesario para crear/actualizar
        private static void Validate(OrganizationalUnitDtoRequest dto)
        {
            var errors = new List<string>();
            if (dto == null) throw new ValidationException("La UO no puede ser nula.");
            // Ajusta reglas según tu dominio:
            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add("La descripción es obligatoria.");

            if (errors.Any())
                throw new ValidationException(string.Join(" | ", errors));
        }

        public override async Task<OrganizationalUnitDto> Save(OrganizationalUnitDtoRequest dto)
        {
            try
            {
                Validate(dto);

                var entity = _mapper.Map<OrganizationalUnit>(dto);
                var created = await _data.SaveAsync(entity);

                return _mapper.Map<OrganizationalUnitDto>(created);
            }
            catch (ValidationException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear OrganizationalUnit.");
                throw new ExternalServiceException("Base de datos", "No se pudo crear la Unidad Organizativa.");
            }
        }

        public override async Task<bool> Update(OrganizationalUnitDtoRequest dto)
        {
            try
            {
                if (dto.Id <= 0) throw new ValidationException("Id inválido para actualizar.");
                Validate(dto);

                var entity = _mapper.Map<OrganizationalUnit>(dto);
                return await _data.UpdateAsync(entity);
            }
            catch (ValidationException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar OrganizationalUnit con Id {Id}.", dto.Id);
                throw new ExternalServiceException("Base de datos", "No se pudo actualizar la Unidad Organizativa.");
            }
        }

        // Conteo de divisiones
        public Task<int> CountDivisionAsync(int organizationalUnitId) =>
            _divisionData.CountByOrgUnitAsync(organizationalUnitId);

        //Conteo de branch

        public Task<int> CountBranchesAsync(int organizationalUnitId) =>
            _branchData.CountBranchesByOrgUnitAsync(organizationalUnitId);
    }
}
