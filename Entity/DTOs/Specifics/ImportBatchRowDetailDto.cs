using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    /// <summary>
    /// DTO para consultar el detalle de una fila procesada en un lote de importación.
    /// </summary>
    public class ImportBatchRowDetailDto
    {
        /// <summary>Identificador único de la fila registrada.</summary>
        public int Id { get; set; }

        /// <summary>Número de fila en el archivo Excel.</summary>
        public int RowNumber { get; set; }

        /// <summary>Indica si la fila se procesó con éxito.</summary>
        public bool Success { get; set; }

        /// <summary>Mensaje de resultado (ejemplo: “Importación exitosa”, “Email ya existe”).</summary>
        public string? Message { get; set; }

        /// <summary>Identificador de la persona creada (si aplica).</summary>
        public int? PersonId { get; set; }

        /// <summary>Identificador de la vinculación Persona–División–Perfil (si aplica).</summary>
        public int? PersonDivisionProfileId { get; set; }

        /// <summary>Identificador del carnet generado (si aplica).</summary>
        public int? CardId { get; set; }

        /// <summary>Indica si se actualizó la foto de la persona.</summary>
        public bool UpdatedPhoto { get; set; }
    }
}
