using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics.Cards;
using Entity.Models.Organizational.Assignment;

namespace Data.Interfases.Organizational.Assignment
{
    public interface IIssuedCardData : ICrudBase<IssuedCard>
    {
        /// <summary>
        /// Actualiza la URL pública del PDF del carnet en la base de datos.
        /// </summary>
        Task<IssuedCard> UpdatePdfUrlAsync(int cardId, string pdfUrl);

        /// <summary>
        /// Consulta la información completa del carnet, incluyendo organización y sucursal (vía OrganizationalUnitBranch).
        /// </summary>
        Task<CardUserData> GetCardDataByIssuedIdAsync(int issuedCardId);
    }
}
