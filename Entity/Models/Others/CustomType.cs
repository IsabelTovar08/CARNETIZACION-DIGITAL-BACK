using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Event;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
using Entity.Models.Organizational;

namespace Entity.Models.Others
{
    public class CustomType : GenericModel
    {
        public string? Description { get; set; }
        public int TypeCategoryId { get; set; }
        public TypeCategory TypeCategory { get; set; }


        public List<Notification>? Notifications { get; set; }
        public List<Person>? PersonsDocumentType { get; set; }
        public List<Organization>? Organization { get; set; }
        public List<AccessPoint>? accessPoints { get; set; }

    }
}
