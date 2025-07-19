using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Organization;

namespace Entity.Models
{
    public class City : GenericModel
    {
        public string Name { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Branch> Branches { get; set; }
    }
}
