using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class ImportContextCard
    {
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; } = default!;

        public int OrganizationalUnitId { get; set; }
        public string OrganizationalUnitCode { get; set; } = default!;

        public int InternalDivisionId { get; set; }
        public string InternalDivisionCode { get; set; } = default!;


        public int CardTemplateId { get; set; }
        public string CardTemplateCode { get; set; } = default!;


        public int ProfileId { get; set; }                // Perfil de la división
        public DateTime ValidFrom { get; set; }           // Inicio de vigencia del carnet
        public DateTime ValidTo { get; set; }             // Fin de vigencia del carnet
    }
}
