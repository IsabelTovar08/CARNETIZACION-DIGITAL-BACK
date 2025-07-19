using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Event
{
    public class AccessPoint : GenericModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
