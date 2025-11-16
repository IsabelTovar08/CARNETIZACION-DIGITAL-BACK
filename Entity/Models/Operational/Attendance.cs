using System;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Operational;

namespace Entity.Models.Organizational
{
    public class Attendance : BaseModel
    {
        public DateTime TimeOfEntry { get; set; }
        public DateTime? TimeOfExit { get; set; }

        public int EventAccessPointEntryId { get; set; }
        public int? EventAccessPointExitId { get; set; }

        public EventAccessPoint EventAccessPointEntry { get; set; }
        public EventAccessPoint? EventAccessPointExit { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
