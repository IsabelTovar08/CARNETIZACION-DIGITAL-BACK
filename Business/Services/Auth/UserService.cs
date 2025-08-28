using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Utilities.Helper.EncryptedPassword;

namespace Business.Services.Auth
{
        public class UserService
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<UserService> _logger;
            private readonly INotify _notifyServer;

            public UserService(ApplicationDbContext context, ILogger<UserService> logger, INotify emailSender)
            {
                _context = context;
                _logger = logger;
                _notifyServer = emailSender;
            }


            public async Task SendEmailWelcome(User user)
            {
                var subject = "Bienvenido a nuestra plataforma";
                var body = $@"
                <!DOCTYPE html>
                <html lang='es'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            background-color: #f9f9f9;
                            color: #333;
                            padding: 40px 20px;
                            margin: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            border-radius: 12px;
                            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
                            padding: 30px;
                            text-align: center;
                        }}
                        h1 {{
                            color: #2a9d8f;
                            margin-bottom: 10px;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.6;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 12px;
                            color: #999;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>¡Bienvenido, {user.UserName}!</h1>
                        <p>Gracias por registrarte. Estamos felices de tenerte con nosotros.</p>
                        <p>Esperamos que disfrutes tu experiencia. Si tienes alguna pregunta, no dudes en escribirnos.</p>
                        <div class='footer'>
                            © {DateTime.Now.Year} ModelSecurityIsabel. Todos los derechos reservados.
                        </div>
                    </div>
                </body>
                </html>";

                await _notifyServer.NotifyAsync("Email", user.UserName, subject, body);
            }


            

        
    }
}
