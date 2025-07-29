using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class PersonDtoRequest : BaseDTO
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public int? DocumentTypeId { get; set; }

        public string Identification { get; set; }
        public int? BloodTypeId { get; set; }

        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public int CityId { get; set; }
    }
}
