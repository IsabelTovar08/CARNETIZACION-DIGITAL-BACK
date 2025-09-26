using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class RolDto : GenericDto
    {
        public string? Description { get; set; }
    }
}
