using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.Models.Organizational.Structure
{
    public class AreaCategory : GenericModel
    {
        public string? Description { get; set; }

        public List<InternalDivision>? InternalDivisions { get; set; }
    }
}
