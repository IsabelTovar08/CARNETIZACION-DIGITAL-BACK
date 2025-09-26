using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;

namespace Business.Interfaces.Operational
{
    public interface ICardTemplateBusiness : IBaseBusiness<CardTemplate, CardTemplateRequest, CardTemplateResponse>
    {
    }
}
