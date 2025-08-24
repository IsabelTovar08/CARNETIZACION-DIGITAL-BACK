using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Organizational.Structure;
namespace Entity.Models.Organizational.Assignment

{
    public class PersonDivisionProfile : BaseModel
    {
        public Card Card { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int ProfileId { get; set; }
        public Profiles Profile { get; set; }

        public int InternalDivisionId { get; set; }
        public InternalDivision InternalDivision { get; set; }


        public bool IsCurrentlySelected { get; set; }
    }

}