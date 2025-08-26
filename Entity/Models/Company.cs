using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; } 
        public string ConnectionString { get; set; } 
        public bool IsActive { get; set; }
    }

}
