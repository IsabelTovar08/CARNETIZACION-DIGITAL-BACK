using System;
using Entity.Models.Base;
using Entity.Models.Organization;

namespace Entity.Models.Event
{
    public class Attendance : GenericModel
    {
        public DateTime TimeOfEntry { get; set; }
        public DateTime TimeOfExit { get; set; }
        public string PointOfEntryAccess { get; set; }
        public string PointOfExitAccess { get; set; }

        public int AccessPointId { get; set; }
        public AccessPoint AccessPoint { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
