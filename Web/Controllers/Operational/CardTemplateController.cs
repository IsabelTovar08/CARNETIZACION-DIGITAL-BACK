using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using ExCSS;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class CardTemplateController : GenericController<CardTemplate, CardTemplateRequest, CardTemplateResponse>
    {
        protected readonly ICardTemplateBusiness _business;
        public CardTemplateController(ICardTemplateBusiness business, ILogger<CardTemplateController> logger,
            ICardTemplateBusiness cardTemplateBusiness) : base(business, logger)
        {
            _business = cardTemplateBusiness;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public override async Task<IActionResult> Create([FromForm] CardTemplateRequest dto)
        {
            return await base.Create(dto);

        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async override Task<IActionResult> Update([FromForm] CardTemplateRequest dto)
        {
            return await base.Update(dto);
        }
    }
}
