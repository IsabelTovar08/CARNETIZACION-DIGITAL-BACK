using Data.Classes.Base;
using Entity.Context;
using Entity.Models;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class ModuleData : BaseData<Module>
    {
        public ModuleData(ApplicationDbContext context, ILogger<Module> logger) : base(context, logger)
        {

        }

    }
}