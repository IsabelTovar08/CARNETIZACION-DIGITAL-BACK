using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class AccessPointBusiness : BaseBusiness<AccessPoint, AccessPointDto, AccessPointDto>, IAccessPointBusiness
    {
        public AccessPointBusiness(IAccessPointData data, ILogger<AccessPoint> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }
}
