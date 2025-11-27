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
using Entity.Models.Operational;
using Entity.Models.Organizational.Assignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Organizational.Assignment
{
    public class CardConfigurationData : BaseData<CardConfiguration>, ICardConfigurationData
    {
        public CardConfigurationData(ApplicationDbContext context, ILogger<CardConfiguration> logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<CardConfiguration>> GetAllAsync()
        {
            var cards = await _context.Set<CardConfiguration>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Include(c => c.CardTemplate)
                .Include(c => c.Profile)
                .Include(c => c.CardTemplate)
                .ToListAsync();

            return cards;
        }

        public override async Task<IEnumerable<CardConfiguration>> GetActiveAsync()
        {
            var cards = await _context.Set<CardConfiguration>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Include(c => c.CardTemplate)
                .Include(c => c.Profile)
                .Include(c => c.CardTemplate)
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return cards;
        }

        //public override Task<CardConfiguration> SaveAsync(CardConfiguration entity)
        //{
        //    entity.QRCode = "123";
        //    entity.UniqueId = new Guid();
        //    return base.SaveAsync(entity);
        //}



        /// <summary>
        /// Retorna el total de carnets que no están eliminados lógicamente
        /// </summary>
        /// <returns>Total de carnets</returns>
        public async Task<int> getTotalNumberOfIDCardConfigurations()
        {
            try
            {
                // Contar los registros en la tabla CardConfiguration donde IsDeleted sea false
                var total = await _context.Set<CardConfiguration>()
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


        /// /<inheritdoc/>
        public async Task<CardTemplate> GetTemplateByCardConfigurationIdAsync(int cardConfigurationId)
        {
            try
            {
                var configuration = await _context.CardsConfigurations
                    .Include(c => c.CardTemplate)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == cardConfigurationId && !c.IsDeleted);

                if (configuration == null)
                    throw new InvalidOperationException($"No se encontró el CardConfiguration con ID {cardConfigurationId}.");

                return configuration.CardTemplate!;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error controlado al obtener la plantilla por CardConfigurationId {CardConfigurationId}", cardConfigurationId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la plantilla asociada al CardConfigurationId {CardConfigurationId}", cardConfigurationId);
                throw new InvalidOperationException("Ocurrió un error al consultar la plantilla del carnet.", ex);
            }
        }

    }

}
