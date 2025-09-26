using Business.Interfases;
using Data.Implementations.Organizational.Assignment;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Organizational.Assignment
{
    public interface ICardBusiness : IBaseBusiness<Card,CardDtoRequest,CardDto>
    {
    }
}
