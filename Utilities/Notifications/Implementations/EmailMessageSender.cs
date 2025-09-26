using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Notifications;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Options;

namespace Utilities.Notifications.Implementations
{
    public class EmailMessageSender : IMessageSender
    {
        public readonly EmailSettings _settings;

        public EmailMessageSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendMessageAsync(string to, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new InvalidOperationException("Destinatario vacío.");

            var addr = new System.Net.Mail.MailAddress(to.Trim()); // lanza si es inválido
            var toOk = addr.Address;

            using var smtpClient = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
                EnableSsl = _settings.EnableSsl
            };

            using var mailMessage = new MailMessage(_settings.SenderEmail, toOk)
            {
                Subject = subject ?? string.Empty,
                Body = message ?? string.Empty,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
