using Entity.Models.Base;

namespace Entity.Models.Organizational.Assignment
{
    public class Profiles : GenericModel
    {
        public string? Description { get; set; }
        public List<IssuedCard>? IssuedCards { get; set; }
    }
}
