using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entity.DTOs.Notifications;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Options;

namespace Utilities.Notifications.Implementations
{
    public class TelegramMessageSender : IMessageSender
    {
        private readonly TelegramSettings _settings;
        private readonly HttpClient _httpClient;

        public TelegramMessageSender(IOptions<TelegramSettings> options)
        {
            _settings = options.Value;
            _httpClient = new HttpClient();
        }

        public async Task SendMessageAsync(string to, string subject, string message)
        {
            var fullMessage = $"📢 {subject}\n\n{message}";

            var url = $"https://api.telegram.org/bot{_settings.BotToken}/sendMessage";

            var content = new
            {
                chat_id = to, // ID del usuario o grupo al que vas a enviar el mensaje
                text = fullMessage,
                parse_mode = "HTML"
            };

            var json = JsonSerializer.Serialize(content);
            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); // lanza excepción si no fue 200 OK
        }
    }
}
