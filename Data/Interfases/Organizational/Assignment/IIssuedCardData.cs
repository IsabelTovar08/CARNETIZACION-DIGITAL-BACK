using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Organizational.Assignment
{
    public interface IIssuedCardData : ICrudBase<IssuedCard>
    {
        /// <summary>
        /// Actualiza la URL pública del PDF del carnet en la base de datos.
        /// </summary>
        Task<IssuedCard> UpdatePdfUrlAsync(int cardId, string pdfUrl);
    }
}
