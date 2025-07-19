using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Models.Base;
using Entity.Models.Organization;

namespace Entity.Models
{
    public class User : GenericModel
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
