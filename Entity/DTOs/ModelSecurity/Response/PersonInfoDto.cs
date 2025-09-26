using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class PersonInfoDto : BaseDTO
    {
        public PersonDto PersonalInfo { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int DivissionId { get; set; }
        public string DivissionName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
