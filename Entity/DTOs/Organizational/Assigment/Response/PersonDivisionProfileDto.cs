using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Assigment.Response
{
    public class PersonDivisionProfileDto : BaseDTO
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }

        public bool IsCurrentlySelected { get; set; }


    }
}
