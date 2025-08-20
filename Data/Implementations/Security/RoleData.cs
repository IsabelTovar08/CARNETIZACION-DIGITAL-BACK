
using Data.Classes.Base;
using Entity;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Request;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class RoleData : BaseData<Role>
    {
        public RoleData(ApplicationDbContext context, ILogger<Role> logger) : base(context, logger)
        {

        }

    }
}