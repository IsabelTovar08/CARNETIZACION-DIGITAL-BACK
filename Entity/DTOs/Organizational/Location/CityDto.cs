using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Location
{
    public class CityDto : GenericBaseDto
    {
        public int DeparmentId { get; set; }
        public string? DeparmentName { get; set; }
    }
}
