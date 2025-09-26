using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Assignment;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class MassiveInfluxOfPeople
    {
        public int ProfileId { get; set; }
        public int InternalDivissionId { get; set; }

        //public ImportExcelRequest ExcelRequest { get; set; }
        public IFormFile File { get; set; }

    }
}
