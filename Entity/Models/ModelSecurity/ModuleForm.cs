using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.Models
{
    public class ModuleForm : BaseModel
    {
        public int ModuleId { get; set; }
        public int FormId { get; set; }

        public Module Module { get; set; }
        public Form Form { get; set; }
    }
}
