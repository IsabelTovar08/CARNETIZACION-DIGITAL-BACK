using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Event;
using Entity.Models.Organizational;
using Entity.Models.Others;

namespace Entity.Models.ModelSecurity
{
    public class Person : BaseModel
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DocumenTypeId { get; set; }

        public string? BlodType { get; set; }
        public string? Photo { get; set; }

       
        public int? CityId { get; set; }

        public User? User { get; set; }
        public City? City { get; set; }

        public List<Attendance>? Attendances { get; set; }
        public PersonDivisionProfile? PersonDivisionProfile { get; set; }
        public CustomType? DocumenType { get; set; }
    }
}
