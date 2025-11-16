using Entity.Models.Operational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Operational
{
    public interface IEventAccessPointData : ICrudBase<EventAccessPoint>
    {
        /// <summary>
        /// Verifica si ya existe una relación EventId + AccessPointId.
        /// ignoreId permite excluir el registro actual al actualizar.
        /// </summary>
        Task<bool> ExistsDuplicateAsync(int eventId, int accessPointId);
        /// <summary>
        /// Obtiene un EventAccessPoint por su QrCodeKey.
        /// </summary>
        Task<EventAccessPoint?> GetByQrKeyAsync(string qrKey);

    }
}
