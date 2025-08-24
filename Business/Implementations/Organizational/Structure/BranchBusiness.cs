using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Structure;
using Data.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Organizational.Structure
{
    public class BranchBusiness : BaseBusiness<Branch, BranchDtoRequest, BranchDto>, IBranchBusiness
    {
        public BranchBusiness (ICrudBase<Branch> data, ILogger<Branch> logger, IMapper mapper) : base(data, logger, mapper)
        { 
        } 
    }
}
