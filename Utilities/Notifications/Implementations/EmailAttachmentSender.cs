using Entity.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utilities.Notifications.Interfases;

namespace Utilities.Notifications.Implementations
{
    public class EmailAttachmentSender : IEmailAttachmentSender
    {
        private readonly EmailSettings _settings;

        public EmailAttachmentSender(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailWithAttachmentAsync(
            string to,
            string subject,
            string body,
            byte[] attachmentBytes,
            string attachmentName = "reporte.pdf")
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
                EnableSsl = _settings.EnableSsl
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            // Adjuntar PDF
            if (attachmentBytes != null)
            {
                var ms = new MemoryStream(attachmentBytes);
                message.Attachments.Add(new Attachment(ms, attachmentName));
            }

            await client.SendMailAsync(message);
        }
    }
}
