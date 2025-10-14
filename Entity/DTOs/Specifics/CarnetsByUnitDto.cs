using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    /// <summary>
    /// DTO para Carnets por Unidad Organizativa.
    /// </summary>
    public class CarnetsByUnitDto
    {
        public int UnidadOrganizativaId { get; set; }
        public string UnidadOrganizativa { get; set; }
        public int TotalCarnets { get; set; }
    }
}
