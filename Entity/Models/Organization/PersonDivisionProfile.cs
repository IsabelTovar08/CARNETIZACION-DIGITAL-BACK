using Entity.Models.Base;
using Entity.Models.Organization;
namespace Entity.Models

{
    public class PersonDivisionProfile : GenericModel

    {
        public int CardId { get; set; }
        public Card Card { get; set; }



        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int ProfileId { get; set; }
        public bool IsCurrentlySelected { get; set; }

    }

}