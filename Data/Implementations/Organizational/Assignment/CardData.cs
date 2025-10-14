using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Assignment;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entity.Context;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
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

        public override Task<Card> SaveAsync(Card entity)
        {
            entity.QRCode = "123";
            return base.SaveAsync(entity);
        }

        /// <summary>
        /// Carnets emitidos agrupados por Unidad Organizativa.
        /// </summary>
        public async Task<List<CarnetsByUnitDto>> GetCarnetsByOrganizationalUnitAsync()
        {
            return await _context.OrganizationalUnits
                .GroupJoin(
                    _context.Cards.Where(c => !c.IsDeleted),
                    u => u.Id,
                    c => c.PersonDivisionProfile.InternalDivision.OrganizationalUnitId,
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
            return await _context.Cards
                .Where(c => !c.IsDeleted &&
                            c.PersonDivisionProfile.InternalDivision.OrganizationalUnitId == organizationalUnitId)
                .GroupBy(c => c.PersonDivisionProfile.InternalDivision.Name)
                .Select(g => new CarnetsByDivisionDto
                {
                    DivisionInterna = g.Key,
                    TotalCarnets = g.Count()
                })
                .OrderByDescending(x => x.TotalCarnets)
                .ToListAsync();
        }


        /// <summary>
        /// Carnets emitidos agrupados por Jornada (usando Schedule en Card).
        /// </summary>
        public async Task<List<CarnetsBySheduleDto>> GetCarnetsBySheduleAsync()
        {
            return await _context.Cards
                .Where(c => !c.IsDeleted)
                .Include(c => c.PersonDivisionProfile)
                .GroupBy(c => c.SheduleId) 
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
        public async Task<int> getTotalNumberOfIDCards()
        {
            try
            {
                // Contar los registros en la tabla Card donde IsDeleted sea false
                var total = await _context.Set<Card>()
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

    }
}
