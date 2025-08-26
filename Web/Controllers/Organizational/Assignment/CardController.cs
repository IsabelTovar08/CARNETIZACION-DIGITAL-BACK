using Business.Interfaces.Organizational.Assignment;
using Business.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Assignment
{
    public class CardController : GenericController<Card, CardDtoRequest, CardDto>
    {
        public CardController(ICardBusiness business, ILogger<CardController> logger)
            : base(business, logger)
        { 
        }
    }
}
