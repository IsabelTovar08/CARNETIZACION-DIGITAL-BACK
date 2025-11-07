using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Specifics.Cards;
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


            var prueba = await GetCardDataByIssuedIdAsync(1);
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

        /// </<inheritdoc/>>
        public async Task<CardUserData> GetCardDataByIssuedIdAsync(int issuedCardId)
        {
            var issuedCard = await _context.IssuedCards
                .Include(x => x.Person)
                .Include(x => x.Card)
                    .ThenInclude(c => c.CardTemplate)
                .Include(x => x.Profile)
                .Include(x => x.InternalDivision)
                    .ThenInclude(d => d.AreaCategory)
                .Include(x => x.InternalDivision)
                    .ThenInclude(d => d.OrganizationalUnit)
                        .ThenInclude(u => u.OrganizationalUnitBranches)
                            .ThenInclude(oub => oub.Branch)
                                .ThenInclude(b => b.Organization)
                .FirstOrDefaultAsync(x => x.Id == issuedCardId);

            if (issuedCard == null)
                throw new InvalidOperationException($"No se encontró el carnet emitido con ID {issuedCardId}");

            var division = issuedCard.InternalDivision;
            var unit = division?.OrganizationalUnit;

            // 🔹 Obtener la primera relación con Branch
            var orgUnitBranch = unit?.OrganizationalUnitBranches?.FirstOrDefault();
            var branch = orgUnitBranch?.Branch;
            var org = branch?.Organization;

            return new CardUserData
            {
                // 🔸 Datos personales
                Name = $"{issuedCard.Person?.FirstName} {issuedCard.Person?.LastName}".Trim(),
                Email = issuedCard.Person?.Email ?? string.Empty,
                PhoneNumber = issuedCard.Person?.Phone ?? string.Empty,
                DocumentNumber = issuedCard.Person.DocumentNumber ?? string.Empty,

                // 🔸 Datos organizacionales
                CompanyName = org?.Name ?? "Sin organización",
                BranchName = branch?.Name ?? "Sin sucursal",
                BranchAddress = branch?.Address ?? "Sin dirección",
                BranchPhone = branch?.Phone ?? string.Empty,
                BranchEmail = branch?.Email ?? string.Empty,
                CategoryArea = division.AreaCategory.Name ?? string.Empty,

                // 🔸 Datos del carnet
                CardId = issuedCard.UniqueId.ToString(),
                Profile = issuedCard.Profile?.Name ?? "Sin perfil",
                InternalDivisionName = division?.Name ?? "N/A",
                OrganizationalUnit = unit.Name ?? "N/A",
                UserPhotoUrl = issuedCard.Person?.PhotoUrl ?? string.Empty,
                LogoUrl = org?.Logo ?? "https://carnetgo.com/logo.png",
                QrUrl = issuedCard.QRCode,
                UniqueId = issuedCard.UniqueId
            };
        }
    }
}
