using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Infrastructure.Notifications.Interfases;

namespace Utilities.Notifications.Implementations
{
    public class Notifier :INotify
    {
        private readonly IEnumerable<IMessageSender> _messageSenders;

        public Notifier(IEnumerable<IMessageSender> messageSender)
        {
            _messageSenders = messageSender;
        }

        public async Task NotifyAsync(string channel, string to, string subject, string message)
        {
            IMessageSender? sender = channel.ToLower() switch
            {
                "email" => _messageSenders.FirstOrDefault(s => s is EmailMessageSender),
                "whatsapp" => _messageSenders.FirstOrDefault(s => s is WhatsAppMessageSender),
                "telegram" => _messageSenders.FirstOrDefault(s => s is TelegramMessageSender),
                _ => null
            };

            if (sender == null)
                throw new ArgumentException($"Canal '{channel}' no soportado.");

            try
            {
                await sender.SendMessageAsync(to, subject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar por {channel}: {ex.Message}");
            }
        }

    }
}
