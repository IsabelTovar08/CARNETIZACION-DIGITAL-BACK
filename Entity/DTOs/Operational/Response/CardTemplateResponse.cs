﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Response
{
    public class CardTemplateResponse : GenericDto
    {
        public string FrontBackgroundUrl { get; set; }
        public string BackBackgroundUrl { get; set; }

        public string FrontElementsJson { get; set; }
        public string BackElementsJson { get; set; }
    }
}
