using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Organizational.Assignment
{
    public class CardData : BaseData<Card>, ICardData
    {
        public CardData(ApplicationDbContext context, ILogger<Card> logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<Card>> GetAllAsync()
        {
            var cards = await _context.Set<Card>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Include(c => c.Status)
                .Include(c => c.PersonDivisionProfile)
                    .ThenInclude(pdp => pdp.Person)
                .Include(c => c.PersonDivisionProfile)
                    .ThenInclude(pdp => pdp.InternalDivision)
                        .ThenInclude(d => d.AreaCategory)
                .Include(c => c.PersonDivisionProfile)
                    .ThenInclude(pdp => pdp.Profile)
                .ToListAsync();

            return cards;
        }

    }
}
