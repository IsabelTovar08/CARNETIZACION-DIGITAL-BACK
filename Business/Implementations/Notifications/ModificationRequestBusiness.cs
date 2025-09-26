using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Notifications;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Data.Interfases.Notifications;
using Entity.DTOs.Notifications.Request;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Notifications
{
    public class ModificationRequestBusiness : BaseBusiness<ModificationRequest, ModificationRequestDtoRequest, ModificationRequestDtoResponse>, IModificationRequestBusiness
    {
        public ModificationRequestBusiness(IModificationRequestData data, ILogger<ModificationRequest> logger, IMapper mapper, ICodeGeneratorService<ModificationRequest>? codeService = null) : base(data, logger, mapper, codeService)
        {
        }
    }
}
