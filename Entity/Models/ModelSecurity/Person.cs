using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Location;
using Entity.Models.Parameter;

namespace Entity.Models.ModelSecurity
{
    public class Person : BaseModel
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DocumentTypeId { get; set; }

        public int? BloodTypeId { get; set; }
        public string? Photo { get; set; }

       
        public int? CityId { get; set; }

        public User? User { get; set; }
        public City? City { get; set; }

        public List<Attendance>? Attendances { get; set; }
        public PersonDivisionProfile? PersonDivisionProfile { get; set; }
        public CustomType? DocumentType { get; set; }
        public CustomType? BloodType { get; set; }
    }
}
