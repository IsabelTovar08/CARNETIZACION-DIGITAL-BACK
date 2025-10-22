using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using System;

namespace Entity.Models.Organizational.Assignment
{
    /// <summary>
    /// Representa el carnet personal de una persona asignada a una división/perfil.
    /// Contiene el QR, el PDF y la referencia a la configuración general del carnet.
    /// </summary>
    public class IssuedCard : BaseModel
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int ProfileId { get; set; }
        public Profiles Profile { get; set; }

        public int InternalDivisionId { get; set; }
        public InternalDivision InternalDivision { get; set; }

        public bool IsCurrentlySelected { get; set; }

        public string QRCode { get; set; }
        public Guid UniqueId { get; set; }

        /// <summary>Ruta del PDF generado.</summary>
        public string? PdfUrl { get; set; }

        /// <summary>Referencia a la configuración base del carnet.</summary>
        public int CardId { get; set; }
        public CardConfiguration Card { get; set; }

        /// <summary>Estado del carnet personal (Activo, Revocado, etc.)</summary>
        public int StatusId { get; set; }
        public Status Status { get; set; }
    }
}
