using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Notifications;
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
            using var smtpClient = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
                EnableSsl = _settings.EnableSsl
            };
            using var mailMessage = new MailMessage(_settings.SenderEmail, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
