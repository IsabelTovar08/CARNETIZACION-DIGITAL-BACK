using System;

namespace Entity.DTOs.Specifics

{
    public class EventFilterDto
    {
        /// <summary>
        /// Estado del evento:
        /// 1 = Activo
        /// 2 = Finalizado
        /// 3 = Cancelado
        /// 4 = Programado
        /// etc...
        /// </summary>
        public int? StatusId { get; set; }

        /// <summary>
        /// Tipo de evento (conferencia, taller, etc.)
        /// </summary>
        public int? EventTypeId { get; set; }

        /// <summary>
        /// true = Público
        /// false = Privado/restringido
        /// null = Ambos
        /// </summary>
        public bool? IsPublic { get; set; }
    }
}
