using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Notifications.Request;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;

namespace Business.Interfaces.Operational
{
    public interface IModificationRequestBusiness : IBaseBusiness<ModificationRequest, ModificationRequestDto, ModificationRequestResponseDto>
    {
        /// <summary>
        /// Aprueba una solicitud de modificación, actualiza la persona y marca la solicitud como aprobada.
        /// </summary>
        /// <param name="requestId">Identificador de la solicitud</param>
        /// <param name="approvalMessage">Mensaje opcional de aprobación</param>
        /// <returns>True si se actualizó correctamente</returns>
        Task<bool> ApproveRequestAsync(int requestId, string? approvalMessage = null);


        /// <summary>
        /// Rechaza una solicitud de modificación, asignando un mensaje opcional.
        /// </summary>
        Task<bool> RejectRequestAsync(int requestId, string? rejectionMessage = null);

        /// <summary>
        /// Obtiene todas las solicitudes realizadas por el usuario autenticado.
        /// </summary>
        Task<IEnumerable<ModificationRequestResponseDto>> GetByCurrentUserAsync();
    }
}
