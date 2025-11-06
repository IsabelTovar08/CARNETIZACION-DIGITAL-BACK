using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Parameter;

namespace Entity.DTOs.Organizational.Assigment.Response
{
    public class IssuedCardDto : BaseDTO
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public int InternalDivisionId { get; set; }
        public string DivisionName { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }

        public bool IsCurrentlySelected { get; set; }

        public string QRCode { get; set; }
        public Guid UniqueId { get; set; }

        public string? PdfUrl { get; set; }
        public int CardId { get; set; }

        public int StatusId { get; set; }


    }
}
