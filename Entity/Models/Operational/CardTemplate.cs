using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.Models.Operational
{
    public class CardTemplate : GenericModel
    {
        public string FrontBackgroundUrl { get; set; }
        public string BackBackgroundUrl { get; set; }

        public string FrontElementsJson { get; set; }
        public string BackElementsJson  { get; set; }
    }
}
