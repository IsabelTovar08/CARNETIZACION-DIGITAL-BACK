using Entity.Models.Base;

namespace Entity.Models.Organizational
{
    public class Profile : GenericModel
    {
        public PersonDivisionProfile? PersonDivisionProfile { get; set; }
    }
}
