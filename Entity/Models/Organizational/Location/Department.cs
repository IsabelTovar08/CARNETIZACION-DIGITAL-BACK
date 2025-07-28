using Entity.Models.Base;

namespace Entity.Models.Organizational.Location
{
    public class Department : GenericModel
    {
        public List<City>? Cities { get; set; }
    }
}
