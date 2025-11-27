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
        private readonly IOrganizationalUnitData _organizationalUnit;
        public OrganizationalUnitBusiness(
            ICrudBase<OrganizationalUnit> data,
            ILogger<OrganizationalUnit> logger,
            IMapper mapper,
            IInternalDivisionData divisionData,   
            IOrganizationalUnitBranchData branchData, 
            IOrganizationalUnitData organizationalUnit
        ) : base(data, logger, mapper)
        {
            _divisionData = divisionData;
            _branchData = branchData;
            _organizationalUnit  = organizationalUnit;
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

        /// <summary>
        /// Para asignar mas de una branch a una unidad organizativa
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task<OrganizationalUnitDto> CreateOrganizationalUnitWithBranchesAsync(OrganizationalUnitCreateWithBranchesDtoRequest dto)
        {
            if (dto == null)
                throw new Exception("La información enviada es inválida.");

            if (dto.Unit == null)
                throw new Exception("Los datos de la unidad organizacional son requeridos.");

            // 1. Crear la unidad organizacional
            var unit = new OrganizationalUnit
            {
                Name = dto.Unit.Name,
                Description = dto.Unit.Description,
                IsDeleted = false,
                CreateAt = DateTime.UtcNow
            };

            await _data.SaveAsync(unit);

            // 2. Validar branches
            if (dto.BranchIds != null && dto.BranchIds.Count > 0)
            {
                foreach (var branchId in dto.BranchIds)
                {
                    var branch = await _branchData.GetByIdAsync(branchId);
                    if (branch == null)
                        throw new Exception($"La sede con ID {branchId} no existe.");

                    var link = new OrganizationalUnitBranch
                    {
                        BranchId = branchId,
                        OrganizationUnitId = unit.Id,
                        CreateAt = DateTime.UtcNow
                    };

                    await _branchData.SaveAsync(link);
                }
            }

          

            // 3. Retornar DTO Response
            return new OrganizationalUnitDto
            {
                Id = unit.Id,
                Name = unit.Name,
                //Code = unit.Code,
                Description = unit.Description,
                DivisionsCount = 0,
                BranchesCount = dto.BranchIds?.Count ?? 0
            };
        }


        /// <summary>
        /// Para que me pueda agragar una branch a una unidad organizativa que ya existe
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddBranchToOrganizationalUnitAsync(OrganizationalUnitBranchDtoRequest dto)
        {
            if (dto == null)
                throw new Exception("La información enviada es inválida.");

            // 1. Validar unidad
            var unit = await _organizationalUnit.GetByIdAsync(dto.OrganizationalUnitId);
            if (unit == null)
                throw new Exception($"La unidad organizacional con ID {dto.OrganizationalUnitId} no existe.");

            // 2. Validar branch
            var branch = await _branchData.GetByIdAsync(dto.BranchId);
            if (branch == null)
                throw new Exception($"La sede con ID {dto.BranchId} no existe.");

            // 3. Validar que no exista ya el vínculo
            var existing = await _branchData.GetLinkAsync(dto.OrganizationalUnitId, dto.BranchId);
            if (existing != null)
                throw new Exception("La sede ya está asignada a esta unidad organizacional.");

            // 4. Crear el vínculo
            var link = new OrganizationalUnitBranch
            {
                OrganizationUnitId = dto.OrganizationalUnitId,
                BranchId = dto.BranchId,
                CreateAt = DateTime.UtcNow
            };

            await _branchData.SaveAsync(link);

            return true;
        }

        /// <summary>
        /// Para remover surcursales de unidades organizativas
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RemoveBranchFromOrganizationalUnitAsync(OrganizationalUnitBranchDtoRequest dto)
        {
            if (dto == null)
                throw new Exception("La información enviada es inválida.");

            // 1. Validar unidad
            var unit = await _organizationalUnit.GetByIdAsync(dto.OrganizationalUnitId);
            if (unit == null)
                throw new Exception($"La unidad organizacional con ID {dto.OrganizationalUnitId} no existe.");

            // 2. Validar branch
            var branch = await _branchData.GetByIdAsync(dto.BranchId);
            if (branch == null)
                throw new Exception($"La sede con ID {dto.BranchId} no existe.");

            // 3. Buscar la relación actual
            var link = await _branchData.GetLinkAsync(dto.OrganizationalUnitId, dto.BranchId);
            if (link == null)
                throw new Exception("La sede NO está asignada a esta unidad organizacional.");

            // 4. Marcar como eliminado lógico
            link.IsDeleted = true;
            link.UpdateAt = DateTime.UtcNow;
                
            await _branchData.UpdateAsync(link);

            return true;
        }

        public async Task<OrganizationalUnitDto> GetByIdFullAsync(int id)
        {
            var entity = await _organizationalUnit.GetFullByIdAsync(id);

            if (entity == null)
                throw new ValidationException("La unidad organizativa no existe.");

            // aquí se aplica el mapping que ya definiste (DivisionsCount / BranchesCount)
            return _mapper.Map<OrganizationalUnitDto>(entity);
        }


    }
}
