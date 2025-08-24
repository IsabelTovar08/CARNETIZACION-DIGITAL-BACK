using Entity.Models.Base;

namespace Entity.Models.Organizational.Assignment
{
    public class Profiles : GenericModel
    {
        public string? Description { get; set; }
        public List<PersonDivisionProfile>? PersonDivisionProfiles { get; set; }
    }
}
