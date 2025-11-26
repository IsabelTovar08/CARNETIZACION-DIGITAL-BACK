using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.Operational.Request
{
    public class CardTemplateRequest : GenericDto
    {
        public string? FrontBackgroundUrl { get; set; }
        public string? BackBackgroundUrl { get; set; }

        public string? FrontElementsJson { get; set; }
        public string? BackElementsJson { get; set; }

        public IFormFile? FrontFile { get; set; }
        public IFormFile? BackFile { get; set; }
    }
}
