using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Notifications;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Utilities.Notifications.Implementations
{
    public class WhatsAppMessageSender : IMessageSender
    {
        private readonly TwilioSettings _settings;

        public WhatsAppMessageSender(IOptions<TwilioSettings> options)
        {
            _settings = options.Value;
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        }

        public async Task SendMessageAsync(string to, string subject, string message)
        {
            var finalMessage = $"{subject}\n {message}";
            await MessageResource.CreateAsync(
                body: finalMessage,
                from: new PhoneNumber(_settings.FromNumber),
                to: new PhoneNumber("whatsApp:" + to)
            );
        }
    }
}
