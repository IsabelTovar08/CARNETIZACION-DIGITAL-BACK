using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Organizational.Structure;

namespace Entity.Models.Organizational.Location
{
    public class City : GenericModel
    {
        public int DeparmentId { get; set; }
        public Department? Department { get; set; }
        public List<Branch>? Branches { get; set; }
    }
}
