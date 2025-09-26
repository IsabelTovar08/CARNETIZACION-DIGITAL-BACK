using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class CardTemplateController : GenericController<CardTemplate, CardTemplateRequest, CardTemplateResponse>
    {
        public CardTemplateController(ICardTemplateBusiness business, ILogger<CardTemplateController> logger) : base(business, logger)
        {
        }
    }
}
