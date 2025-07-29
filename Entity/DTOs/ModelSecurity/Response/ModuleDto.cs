using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class ModuleDto : GenericBaseDto
    {
        public string? Description { get; set; }
    }
}
