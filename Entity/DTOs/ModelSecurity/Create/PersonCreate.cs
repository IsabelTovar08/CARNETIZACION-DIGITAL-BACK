using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.Create
{
    public class PersonCreate : BaseDTO
    {
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        [Required]
        public string? Identification { get; set; }
        [Required]
        public string? Phone { get; set; }
    }
}
