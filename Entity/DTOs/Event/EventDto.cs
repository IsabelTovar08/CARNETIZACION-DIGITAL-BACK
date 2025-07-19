using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Event
{
    public  class EventDto : GenericBaseDto
    {
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public DateTime SceduleDate { get; set; }
        public int AccessPointId { get; set; }
        public string AccessPointName { get; set; }
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public DateTime SceduleTime { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public bool Ispublic { get; set; }
    }
}
