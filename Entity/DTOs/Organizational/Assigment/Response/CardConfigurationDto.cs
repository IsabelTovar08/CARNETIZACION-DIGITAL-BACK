using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Models.Operational;

namespace Entity.DTOs.Organizational.Assigment.Response
{
    public class CardConfigurationDto : GenericDto
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }


        public int ProfileId { get; set; }
        public string? ProfileName { get; set; }


        public int CardTemplateId { get; set; }
        public string CardTemplateName { get; set; }

        /// <summary>Fecha desde la cual los carnets bajo esta configuración son válidos.</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Fecha de expiración general de los carnets.</summary>
        public DateTime ValidTo { get; set; }

    }
}
