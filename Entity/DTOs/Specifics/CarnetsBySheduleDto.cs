using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    /// <summary>
    /// DTO para Carnets por Jornada.
    /// </summary>
    public class CarnetsBySheduleDto
    {
        public string Jornada { get; set; }
        public int TotalCarnets { get; set; }
    }
}
