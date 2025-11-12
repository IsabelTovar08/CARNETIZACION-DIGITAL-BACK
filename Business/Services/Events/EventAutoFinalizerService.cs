using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Events
{
    public class EventAutoFinalizerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventAutoFinalizerService> _logger;

        public EventAutoFinalizerService(IServiceProvider serviceProvider, ILogger<EventAutoFinalizerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EventAutoFinalizerService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var eventRepo = scope.ServiceProvider.GetRequiredService<IEventData>();
                        var eventBusiness = scope.ServiceProvider.GetRequiredService<IEventBusiness>();

                        var now = DateTime.Now;

                        // Trae eventos activos cuyo End ya pasó
                        var expiredEvents = await eventRepo.GetEventsToFinalizeAsync(now);

                        foreach (var ev in expiredEvents)
                        {
                            ev.StatusId = 9;
                            ev.IsPublic = false; // opcional
                            await eventRepo.UpdateAsync(ev);

                            _logger.LogInformation($"Evento {ev.Name} ({ev.Id}) finalizado automáticamente.");
                        }

                        // Verificar eventos "en curso"
                        var allActiveEvents = await eventRepo.GetActiveEventsAsync(); 
                        foreach (var ev in allActiveEvents)
                        {
                            await eventBusiness.CheckAndUpdateEventStatusAsync(ev.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en EventAutoFinalizerService.");
                }

                // Espera 1 minuto antes de volver a revisar
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
