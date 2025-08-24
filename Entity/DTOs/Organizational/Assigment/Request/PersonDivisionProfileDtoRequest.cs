using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class PersonDivisionProfileDtoRequest : BaseDTO
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int DivisionId { get; set; }
        
        [Required]
        public int ProfileId { get; set; }
    }
}
