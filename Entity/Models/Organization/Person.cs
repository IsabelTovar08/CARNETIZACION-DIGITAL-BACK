using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Event;

namespace Entity.Models.Organization
{
    public class Person : GenericModel
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Email { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? DocumenType { get; set; }
        public string? BlodType { get; set; }
        public string? Photo { get; set; }
        public bool IsDeleted { get; set; }


        public int? AttendanceId { get; set; }
        public int? PersonDivisionProfileId { get; set; }

       
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        
        public User? User { get; set; }
        public City? City { get; set; }
        public List<Attendance>? Attendances { get; set; }
        public PersonDivisionProfile? PersonDivisionProfile { get; set; }
        public Organization? Organization { get; set; }
        public Branch? Branch { get; set; }
    }
}
