using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Operational
{
    public class EventAccessPointBusiness : BaseBusiness<EventAccessPoint, EventAccessPointDtoRequest, EventAccessPointDto>, IEventAccessPointBusiness
    {
        protected readonly IEventAccessPointData _data;
        public EventAccessPointBusiness(IEventAccessPointData data, ILogger<EventAccessPoint> logger, IMapper mapper, ICodeGeneratorService<EventAccessPoint>? codeService = null) : base(data, logger, mapper, codeService)
        {
            _data = data;
        }

        ///  <inheritdoc/>
        public async Task<EventAccessPoint?> GetByQrKey(string qrKey)
        {
            try
            {
                return await _data.GetByQrKeyAsync(qrKey);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al consultar el Punto de Acesso del evento a través del UNIQUEKEY: {ex.Message}", ex);
            }
        }
    }
}
