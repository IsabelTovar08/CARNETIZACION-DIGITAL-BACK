using Business.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Assignment
{
    public class IssuedCardController : GenericController<IssuedCard, IssuedCardDtoRequest, IssuedCardDto>
    {
        public IssuedCardController(IBaseBusiness<IssuedCard, IssuedCardDtoRequest, IssuedCardDto> business, ILogger<IssuedCardController> logger) : base(business, logger)
        {
        }
    }
}
