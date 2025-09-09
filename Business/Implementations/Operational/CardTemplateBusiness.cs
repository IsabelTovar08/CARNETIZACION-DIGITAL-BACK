using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class CardTemplateBusiness : BaseBusiness<CardTemplate, CardTemplateRequest, CardTemplateResponse>, ICardTemplateBusiness
    {
        public CardTemplateBusiness(ICrudBase<CardTemplate> data, ILogger<CardTemplate> logger, IMapper mapper, ICodeGeneratorService<CardTemplate>? codeService = null) : base(data, logger, mapper, codeService)
        {
        }
    }
}
