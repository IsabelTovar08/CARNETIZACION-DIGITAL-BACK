using Data.Classes.Base;
using Entity.Context;
using Entity.Models;
using Microsoft.Extensions.Logging;
namespace Data.Classes.Specifics
{
    public class PermissionData : BaseData<Permission>
    {
        public PermissionData(ApplicationDbContext context, ILogger<Permission> logger) : base(context, logger)
        {

        }
    }
}