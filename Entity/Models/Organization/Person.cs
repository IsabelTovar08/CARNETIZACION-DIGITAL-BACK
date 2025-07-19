using System.ComponentModel.DataAnnotations;
using Entity.Models;
using Entity.Models.Base;

namespace Entity.Models
{
    public class Person : BaseModel
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string? Identification { get; set; }
        public string? Phone { get; set; }
        public List<User> Users { get; set; }
    }
}
