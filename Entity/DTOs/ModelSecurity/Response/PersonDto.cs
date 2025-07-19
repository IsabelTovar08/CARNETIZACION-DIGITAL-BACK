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
        public string Email { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int OrganizationId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationalUnitId { get; set; }
        public string OrganizationalUnitName { get; set; }
        public int InternalDivisionId { get; set; }
        public string InternalDivisionName { get; set; }
        public string Photo { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
    }
}
