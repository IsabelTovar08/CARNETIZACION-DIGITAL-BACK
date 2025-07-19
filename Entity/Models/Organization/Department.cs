using Entity.Models.Base;

namespace Entity.Models
{
    public class Department : GenericModel
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
