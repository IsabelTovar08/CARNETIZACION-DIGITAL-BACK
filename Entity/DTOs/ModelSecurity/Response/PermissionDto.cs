
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class PermissionDto : BaseDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}