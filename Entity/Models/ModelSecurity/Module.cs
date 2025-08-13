using Entity.Models.Base;

namespace Entity.Models
{
    public class Module : GenericModel
    {
        public string? Description { get; set; }

        public List<Form> Forms { get; set; } 
    }
}
