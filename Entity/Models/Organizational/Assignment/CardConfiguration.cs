using System;
using Entity.Models.Base;
using Entity.Models.Operational;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational.Assignment
{
    /// <summary>
    /// Define la configuración base de un conjunto de carnets.
    /// Contiene la plantilla, vigencia y reglas generales.
    /// </summary>
    public class CardConfiguration : BaseModel
    {
        /// <summary>Fecha desde la cual los carnets bajo esta configuración son válidos.</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Fecha de expiración general de los carnets.</summary>
        public DateTime ValidTo { get; set; }

        /// <summary>Plantilla SVG utilizada en la generación del carnet.</summary>
        public int CardTemplateId { get; set; }
        public CardTemplate CardTemplate { get; set; }

        /// <summary>Horario o tipo de emisión asociado.</summary>
        public int SheduleId { get; set; }
        public Schedule Shedule { get; set; }
    }
}
