using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organizational.Assignment
{
    public class ProfileData : BaseData<Profiles>, IProfileData
    {
        public ProfileData(ApplicationDbContext context, ILogger<Profiles> logger) : base(context, logger) 
        {
        }
    }
}
