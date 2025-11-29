using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Entity.DTOs.Specifics.Cards;
using Entity.Models.Organizational.Assignment;

namespace Business.Interfaces.Organizational.Assignment
{
    public interface IIssuedCardBusiness : IBaseBusiness<IssuedCard, IssuedCardDtoRequest, IssuedCardDto>
    {
        /// <summary>
        /// Actualiza la URL pública del PDF del carnet en la base de datos.
        /// </summary>
        Task<IssuedCardDto> UpdatePdfUrlAsync(int cardId, string pdfUrl);

        /// <summary>
        /// Consulta la información completa del carnet, incluyendo organización y sucursal (vía OrganizationalUnitBranch).
        /// </summary>
        Task<CardUserData> GetCardDataByIssuedId(int issuedCardId);

        /// <summary>
        /// Genera el PDF del carnet emitido y devuelve su contenido en Base64.
        /// </summary>
        Task<byte[]> GenerateCardPdfBase64Async(int issuedCardId);

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
        Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync();

        /// <summary>
        /// Retorna carnets emitidos agrupados por División Interna
        /// de una Unidad Organizativa específica.
        /// </summary>
        Task<List<CarnetsByDivisionDto>> GetCarnetsByInternalDivisionAsync(int organizationalUnitId);

        /// <summary>
        /// Obtiene el listado de carnets emitidos agrupados por Jornada (Schedule en Card).
        /// </summary>
        Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync();

        /// <summary>
        /// Retorna el total de carnets activos (no eliminados)
        /// </summary>
        Task<int> GetTotalNumberOfIDCardsAsync();

        /// <summary>
        /// Para traer todos los carnets de las personas
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<IssuedCardDto>> GetIssuedCardsByUserIdAsync(int userId);

    }
}
