using Entity.DTOs.Base;
using Entity.Models.Base;
using System;

namespace Entity.DTOs.Operational.Response
{
    public class AccessPointDtoResponsee : GenericDto
    {
        public string? Description { get; set; }

        public string Name { get; set; } = default!;

        public int EventId { get; set; }
        public string? EventName { get; set; }

        public int TypeId { get; set; }
        public string? Type { get; set; }

        //// NUEVO: expone el QR
        public string? QrCodeKey { get; set; }

       
        public string? Code { get; set; }
    }
}