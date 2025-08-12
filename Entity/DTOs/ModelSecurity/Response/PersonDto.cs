using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;
using Entity.Models.ModelSecurity;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class PersonDto : BaseDTO
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public int? DocumentTypeId { get; set; }    
        public string? DocumentTypeName { get; set; }

        public string DocumentNumber { get; set; }

        public int? BloodTypeId { get; set; }
        public string? BloodTypeName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }

    }
}
