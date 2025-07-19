using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organization
{
    public class PersonDivisionProfileDto : GenericBaseDto
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }

    }
}
