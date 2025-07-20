using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Organizational
{
    public class City : GenericModel
    {
        public int DeparmentId { get; set; }
        public Department Department { get; set; }
        public List<Branch>? Branches { get; set; }
    }
}
