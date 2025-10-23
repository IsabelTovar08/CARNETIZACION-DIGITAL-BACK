using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Organizational.Assignment
{
    public class IssuedCardData : BaseData<IssuedCard>, IIssuedCardData
    {
        public IssuedCardData(ApplicationDbContext context, ILogger<IssuedCard> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<IssuedCard>> GetAllAsync()
        {
            var cards = await _context.Set<IssuedCard>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Include(c => c.Person)
                .Include(c => c.Profile)
                .Include(c => c.InternalDivision)
                .Include(c => c.Card)
                .Include(c => c.Status)
                .ToListAsync();
            return cards;
        }

        /// <summary>
        /// Actualiza la URL pública del PDF del carnet en la base de datos.
        /// </summary>
        public async Task<IssuedCard> UpdatePdfUrlAsync(int cardId, string pdfUrl)
        {
            var card = await _context.IssuedCards.FirstOrDefaultAsync(x => x.Id == cardId);
            if (card == null)
                throw new Exception($"Card with ID {cardId} not found.");

            card.PdfUrl = pdfUrl;
            await _context.SaveChangesAsync();
            return card;
        }


        public override Task<IssuedCard> SaveAsync(IssuedCard entity)
        {
            //entity.QRCode = "123";
            //entity.UniqueId = new Guid();
            return base.SaveAsync(entity);
        }
    }
}
