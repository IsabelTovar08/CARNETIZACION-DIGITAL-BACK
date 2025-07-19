using System;
using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Organization;

namespace Entity.Models
{
    public class Card : GenericModel
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string AttendanceId { get; set; }

        public ICollection<PersonDivisionProfile> PersonDivisionProfiles { get; set; }
    }
}
