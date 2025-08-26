using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Assignment;
using Data.Interfases;
using Data.Interfases.Organizational.Assignment;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Organizational.Assignment
{
    public class CardBusiness : BaseBusiness<Card, CardDtoRequest, CardDto>, ICardBusiness
    {
        public CardBusiness(ICardData data, ILogger<Card> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }
}
