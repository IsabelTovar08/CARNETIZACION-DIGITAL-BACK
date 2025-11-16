using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Data.Implementations.Organizational.Assignment;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Entity.Models.Organizational.Assignment;

namespace Business.Interfaces.Organizational.Assignment
{
    public interface ICardConfigurationBusiness : IBaseBusiness<CardConfiguration,CardConfigurationDtoRequest,CardConfigurationDto>
    {

        /// <summary>
        /// Retorna el total de carnets activos (no eliminados)
        /// </summary>
        Task<int> GetTotalNumberOfIDCardsAsync();
    }
}
