using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Event
{
    public class Attendance : GenericBaseDto
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime TimeOfEntry { get; set; }
        public DateTime TimeOfExit { get; set; }
        public int PointOfEntryAccess { get; set; }
        public int PointOfExitAccess { get; set; }
    }
}
