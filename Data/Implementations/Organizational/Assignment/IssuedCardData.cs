using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using Entity.Context;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Specifics;
using Entity.DTOs.Specifics.Cards;
using Entity.Enums.Extensions;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Specifics;
using static Utilities.Helpers.BarcodeHelper;

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
                .Include(c => c.Shedule)
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


        /// <summary>
        /// Guarda un carnet garantizando:
        /// - Si no hay ninguno seleccionado, este se vuelve seleccionado.
        /// - Si ya hay uno seleccionado, este se crea con IsCurrentlySelected = false.
        /// </summary>
        public override async Task<IssuedCard> SaveAsync(IssuedCard entity)
        {
            // Obtener si existe algún carnet seleccionado de esta persona
            bool alreadySelected = await _context.IssuedCards
                .AnyAsync(c => c.PersonId == entity.PersonId && c.IsCurrentlySelected);

            if (!alreadySelected)
            {
                // Si no hay seleccionado → este se vuelve el seleccionado
                entity.IsCurrentlySelected = true;
            }
            else
            {
                // Si ya existe uno seleccionado → este NO puede quedar seleccionado
                entity.IsCurrentlySelected = false;
            }

            // Guardar normalmente
            return await base.SaveAsync(entity);
        }

        /// </<inheritdoc/>> 
        public async Task<CardUserData> GetCardDataByIssuedIdAsync(int issuedCardId)
        {
            var issuedCard = await _context.IssuedCards
                .Include(x => x.Person)
                .Include(x => x.Card)
                    .ThenInclude(c => c.CardTemplate)
                .Include(x => x.Card)
                    .ThenInclude(c => c.Profile)
                .Include(x => x.Shedule)
                .Include(x => x.InternalDivision)
                    .ThenInclude(d => d.AreaCategory)
                .Include(x => x.InternalDivision)
                    .ThenInclude(d => d.OrganizationalUnit)
                        .ThenInclude(u => u.OrganizationalUnitBranches)
                            .ThenInclude(oub => oub.Branch)
                                .ThenInclude(b => b.Organization)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == issuedCardId);

            if (issuedCard == null)
                throw new InvalidOperationException($"No se encontró el carnet emitido con ID {issuedCardId}");

            var division = issuedCard.InternalDivision;
            var unit = division?.OrganizationalUnit;

            var orgUnitBranch = unit?.OrganizationalUnitBranches?.FirstOrDefault();
            var branch = orgUnitBranch?.Branch;
            var org = branch?.Organization;

            // Logo Base64 limpio
            string base64Logo = CleanBase64Logo(org?.Logo);

            return new CardUserData
            {
                Id = issuedCardId,
                // Datos personales
                Name = $"{issuedCard.Person?.FirstName} {issuedCard.Person?.MiddleName} {issuedCard.Person?.LastName} {issuedCard.Person?.SecondLastName} ".Trim(),
                Email = issuedCard.Person?.Email ?? string.Empty,
                PhoneNumber = issuedCard.Person?.Phone ?? string.Empty,
                DocumentNumber = issuedCard.Person?.DocumentNumber ?? string.Empty,
                DocumentCode = issuedCard.Person?.DocumentType.GetAcronym() ?? string.Empty,
                DocumentName = issuedCard.Person?.DocumentType.GetDisplayName() ?? string.Empty,
                BloodTypeValue = issuedCard.Person?.BloodType?.GetDisplayName() ?? string.Empty,

                // Datos organizacionales
                CompanyName = org?.Name ?? "Sin organización",
                BranchName = branch?.Name ?? "Sin sucursal",
                BranchAddress = branch?.Address ?? "Sin dirección",
                BranchPhone = branch?.Phone ?? string.Empty,
                BranchEmail = branch?.Email ?? string.Empty,
                CategoryArea = division?.AreaCategory?.Name ?? string.Empty,

                // Datos del carnet
                CardId = issuedCard.UniqueId.ToString(),
                Profile = issuedCard.Card.Profile?.Name ?? "Sin perfil",
                InternalDivisionName = division?.Name ?? "N/A",
                OrganizationalUnit = unit?.Name ?? "N/A",
                UserPhotoUrl = issuedCard.Person?.PhotoUrl ?? string.Empty,
                SheduleName = issuedCard.Shedule.Name ?? "N/A",
                // Aquí queda el Base64 limpio del logo
                LogoUrl = base64Logo,

                QrUrl = issuedCard.QRCode,
                UniqueId = issuedCard.UniqueId,
                FrontTemplateUrl = issuedCard.Card.CardTemplate.FrontBackgroundUrl,
                BackTemplateUrl = issuedCard.Card.CardTemplate.BackBackgroundUrl,
                ValidFrom = issuedCard.Card.ValidFrom,
                ValidUntil = issuedCard.Card.ValidTo,
                IssuedDate = issuedCard.Card.CreateAt
            };
        }

        ///// <summary>
        ///// Carnets emitidos agrupados por Unidad Organizativa.
        ///// </summary>
        public async Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync()
            {
                return await _context.OrganizationalUnits
                    .GroupJoin(
                        _context.IssuedCards.Where(c => !c.IsDeleted),
                        u => u.Id,
                        c => c.InternalDivision.OrganizationalUnitId,
                        (unidad, carnets) => new CarnetsByUnitDto
                        {
                            UnidadOrganizativaId = unidad.Id,
                            UnidadOrganizativa = unidad.Name,
                            TotalCarnets = carnets.Count()
                        }
                    )
                    .OrderByDescending(x => x.TotalCarnets)
                    .ToListAsync();
            }

        /// <summary>
        /// Retorna carnets emitidos agrupados por División Interna
        /// de una Unidad Organizativa específica.
        /// </summary>
        public async Task<List<CarnetsByDivisionDto>> GetCarnetsByInternalDivisionAsync(int organizationalUnitId)
        {
            return await _context.IssuedCards
                .Where(c => !c.IsDeleted &&
                            c.InternalDivision.OrganizationalUnitId == organizationalUnitId)
                .GroupBy(c => c.InternalDivision.Name)
                .Select(g => new CarnetsByDivisionDto
                {
                    DivisionInterna = g.Key,
                    TotalCarnets = g.Count()
                })
                .OrderByDescending(x => x.TotalCarnets)
                .ToListAsync();
        }


        ///// <summary>
        ///// Carnets emitidos agrupados por Jornada (usando Schedule en CardConfiguration).
        ///// </summary>
        public async Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync()
        {
            return await _context.IssuedCards
                .Where(c => !c.IsDeleted)
                .GroupBy(c => c.Card.Profile)
                .Select(g => new CarnetsBySheduleDto
                {
                    Jornada = g.Key.ToString(),
                    TotalCarnets = g.Count()
                })
                .OrderByDescending(x => x.TotalCarnets)
                .ToListAsync();
        }

        /// <summary>
        /// Retorna el total de carnets que no están eliminados lógicamente
        /// </summary>
        /// <returns>Total de carnets</returns>
        public async Task<int> GetTotalNumberOfIDCardsAsync()
        {
            try
            {
                // Contar los registros en la tabla CardConfiguration donde IsDeleted sea false
                var total = await _context.Set<IssuedCard>()
                    .Where(c => !c.IsDeleted)
                    .CountAsync();

                return total;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el total de carnets");
                throw;
            }
        }

        /// <summary>
        /// Para mostrar los carnets que tiene la persona 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<List<IssuedCard>> GetIssuedCardsByUserIdAsync(int userId)
        {
            return await _context.IssuedCards
                .Where(x => x.PersonId == userId && !x.IsDeleted)
                .Include(x => x.Person)
                .Include(x => x.Card).ThenInclude(c => c.Profile)
                .Include(x => x.Shedule)
                .Include(x => x.InternalDivision).ThenInclude(d => d.OrganizationalUnit)
                .Include(x => x.Status)
                .OrderByDescending(x => x.CreateAt)
                .AsNoTracking()
                .ToListAsync();
        }


    }

}
