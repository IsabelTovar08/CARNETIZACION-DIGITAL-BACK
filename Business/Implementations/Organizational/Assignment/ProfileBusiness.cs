using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Assignment;
using Data.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Organizational.Assignment
{
    public class ProfileBusiness: BaseBusiness<Profiles, ProfileDtoRequest, ProfileDto>, IProfileBusiness
    {
        public ProfileBusiness(ICrudBase<Profiles> data, ILogger<Profiles> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }
}
