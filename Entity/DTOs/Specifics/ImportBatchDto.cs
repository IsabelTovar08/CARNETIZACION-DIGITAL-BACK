using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    /// <summary>
    /// DTO para consultar información general de un lote de importación.
    /// </summary>
    public class ImportBatchDto
    {
        /// <summary>Identificador único del lote.</summary>
        public int Id { get; set; }

        /// <summary>Nombre del archivo que se importó.</summary>
        public string? FileName { get; set; }

        /// <summary>Origen de la importación (ejemplo: Excel).</summary>
        public string Source { get; set; } = "Excel";

        /// <summary>Usuario que inició la importación.</summary>
        public int? StartedBy { get; set; }
        /// <summary>Usuario que inició la importación.</summary>
        public string? StartedByUserName { get; set; }

        /// <summary>Total de filas detectadas en el archivo.</summary>
        public int TotalRows { get; set; }

        /// <summary>Número de filas exitosas.</summary>
        public int SuccessCount { get; set; }

        /// <summary>Número de filas con error.</summary>
        public int ErrorCount { get; set; }

        /// <summary>Fecha y hora en que inició la importación.</summary>
        public DateTime StartedAt { get; set; }

        /// <summary>Fecha y hora en que terminó la importación (si aplica).</summary>
        public DateTime? EndedAt { get; set; }
    }

}
