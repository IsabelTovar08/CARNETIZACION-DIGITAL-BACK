using System.Collections.Generic;
using System.Xml.Linq;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models.Base;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Location;
using Entity.Models.Parameter;
using Utilities.Enums.Specifics;

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

        public string? PhotoUrl { get; set; }
        public string? PhotoPath { get; set; }


        public int? CityId { get; set; }

        public User? User { get; set; }
        public City? City { get; set; }

        public List<Attendance>? Attendances { get; set; }

        /// <summary>Tarjetas emitidas de esta persona</summary>
        public ICollection<IssuedCard> IssuedCard { get; set; } = new List<IssuedCard>();
        public DocumentType DocumentType { get; set; }
        public BloodType? BloodType { get; set; }
    }
}
