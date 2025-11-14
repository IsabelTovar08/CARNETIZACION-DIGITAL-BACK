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
        /// Obtiene un EventAccessPoint por su QrCodeKey.
        /// </summary>
        Task<EventAccessPoint?> GetByQrKeyAsync(string qrKey);

    }
}
