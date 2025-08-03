using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.Models.Parameter
{
    public class TypeCategory : GenericModel
    {
        public List<CustomType> Types { get; set; }
    }
}
