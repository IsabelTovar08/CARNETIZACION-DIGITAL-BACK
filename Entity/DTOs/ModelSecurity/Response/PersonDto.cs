using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs
{
    public class PersonDto : BaseDTO
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Identification { get; set; }
        public string? Phone { get; set; }
    }
}
