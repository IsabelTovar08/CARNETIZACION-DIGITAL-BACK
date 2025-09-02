using Data.Classes.Base;
using Data.Interfases;
using Data.Interfases.Organizational.Structure;
using Entity.Context;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organizational.Structure
{
    public class OrganizationnalUnitData : BaseData<OrganizationalUnit>, IOrganizationnalUnitData
    {
        public OrganizationnalUnitData(ApplicationDbContext context, ILogger<OrganizationalUnit> logger)  : base(context, logger)
        {
        }


    }
}
