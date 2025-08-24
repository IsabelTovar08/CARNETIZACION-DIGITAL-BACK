using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class CardDtoRequest : GenericBaseDto
    {
        public string Name { get; set; }   
        public string QRCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int StatusId { get; set; }

        public int PersonDivisionProfileId { get; set; }

        public int AreaCategoryId { get; set; }

    }
}
