using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Auth;
using Business.Interfaces.Organizational.Structure;
using Data.Implementations.Organizational.Structure;
using Data.Interfases;
using Data.Interfases.Organizational.Structure;
using Data.Interfases.Security;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Organizational.Structure
{
    public class OrganizationBusiness : BaseBusiness<Organization, OrganizationDtoRequest, OrganizationDto>, IOrganizationBusiness
    {

        private readonly ICurrentUser _currentUser;
        private readonly IPersonData _personData;
        private readonly IOrganizationData _organizationData;
        public OrganizationBusiness(IOrganizationData data, ICurrentUser currentUser, ILogger<Organization> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _organizationData = data;
            _currentUser = currentUser;
        }

        public async Task<OrganizationDto?> GetMyOrganizationAsync()
        {
            var userIdStr = _currentUser.UserIdRaw;
            if (userIdStr == "unknown")
                throw new UnauthorizedAccessException("Usuario no identificado.");

            int userId = int.Parse(userIdStr);

            // 2️⃣ Buscar la organización desde la estructura asignada
            var organization = await _organizationData.GetOrganizationByPersonId(userId);
            if (organization == null)
                throw new KeyNotFoundException("El usuario no está asociado a ninguna organización.");

            return _mapper.Map<OrganizationDto>(organization);
        }

        public async Task<bool> UpdateMyOrganizationAsync(OrganizationUpdateDtoRequest dto)
        {
            var userIdStr = _currentUser.UserIdRaw;

            if (userIdStr == "unknown")
                throw new UnauthorizedAccessException("Usuario no identificado.");

            int userId = int.Parse(userIdStr);

            // 1. Obtener organización de este usuario
            var organization = await _organizationData.GetOrganizationByPersonId(userId);

            if (organization == null)
                throw new KeyNotFoundException("No se encontró organización asociada.");

            // 2. Actualizar datos
            if (dto.Name != null) organization.Name = dto.Name;
            if (dto.Description != null) organization.Description = dto.Description;
            // 3. Guardar cambios
            return await _organizationData.UpdateOrganizationAsync(organization);
        }

    }
}
