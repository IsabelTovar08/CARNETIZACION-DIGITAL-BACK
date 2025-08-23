using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Structure;
using Entity.Context;
using Entity.Models;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Organizational.Structure
{
    public class AreaCategoryData : BaseData<AreaCategory>, IAreaCategoryData                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
    {
        public AreaCategoryData(ApplicationDbContext context, ILogger<AreaCategory> logger): base(context, logger)
        {
           

        }
    }
}
