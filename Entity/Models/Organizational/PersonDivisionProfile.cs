using Entity.Models.Base;
using Entity.Models.ModelSecurity;
namespace Entity.Models.Organizational

{
    public class PersonDivisionProfile : GenericModel
    {
        public Card Card { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int InternalDivisionId { get; set; }
        public InternalDivision InternalDivision { get; set; }


        public bool IsCurrentlySelected { get; set; }
    }

}