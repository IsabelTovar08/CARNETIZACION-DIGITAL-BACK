using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Assigment.Response
{
    public class CardDto : GenericDto
    {
        public string QRCode { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public int PersonDivissionProfileId { get; set; }
        public int PersonId { get; set; }
        public string? PersonFullName { get; set; }

        public int DivisionId { get; set; }
        public string? DivisionName { get; set; }

        public int ProfileId { get; set; }
        public string? ProfileName { get; set; }

        public int AreaCategoryId { get; set; }
        public string? AreaCategoryName { get; set; }

    }
}
