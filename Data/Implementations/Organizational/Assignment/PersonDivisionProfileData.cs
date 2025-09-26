using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organizational.Assignment
{
    public class PersonDivisionProfileData : BaseData<PersonDivisionProfile>, IPersonDivisionProfileData
    {
        public PersonDivisionProfileData(ApplicationDbContext context, ILogger<PersonDivisionProfile> logger) : base(context, logger)
        {
        }
    }
}
