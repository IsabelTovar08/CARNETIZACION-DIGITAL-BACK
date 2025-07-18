using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Models.Base;
namespace Entity.Models
{
    public class User : BaseModel
    {
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public int PersonId { get; set; }

        public Person Person { get; set; }
        public List<UserRoles> UserRoles { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
