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
    public class PersonDivisionProfileBusiness : BaseBusiness<PersonDivisionProfile, PersonDivisionProfileDtoRequest, PersonDivisionProfileDto>, IPersonDivisionProfileBusiness
    {
        public PersonDivisionProfileBusiness(ICrudBase<PersonDivisionProfile> data, ILogger<PersonDivisionProfile> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }
}
