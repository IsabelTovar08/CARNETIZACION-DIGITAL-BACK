using System;
using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational.Assignment
{
    public class Card : BaseModel
    {
        public string QRCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int StatusId { get; set; }


        public int PersonDivissionProfileId { get; set; }
        public PersonDivisionProfile PersonDivisionProfile { get; set; }
        public Status Status { get; set; }

    }
}
