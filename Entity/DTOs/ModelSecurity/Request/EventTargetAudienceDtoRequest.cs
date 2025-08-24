using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Request
{
    public class EventTargetAudienceDtoRequest : GenericBaseDto
    {
        [Required]
        public int TypeId { get; set; }

        [Required]
        public int ReferenceId { get; set; }

        [Required]
        public int EventId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

}