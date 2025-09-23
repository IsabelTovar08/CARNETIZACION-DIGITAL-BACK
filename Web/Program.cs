using Entity.DTOs.Notifications;
using Entity.DTOs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Web.Extensions;
using QuestPDF.Infrastructure;   //  Para QuestPDF
using OfficeOpenXml;            //  Para EPPlus <= 7.x
using Web.Realtime.Hubs;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //  Declarar licencias
            QuestPDF.Settings.License = LicenseType.Community;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger + JWT Bearer
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticación JWT con esquema Bearer. **Pega solo el token (sin 'Bearer ')**.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Servicios personalizados y configuración
            builder.Services.AddProjectServices();
            builder.Services.AddCorsConfiguration(configuration);

            // Automapper
            builder.Services.AddAutoMapper(typeof(Utilities.Helper.MappingProfile));

            // JWT
            builder.Services.AddJwtAuthentication(configuration);

            // Conexión a DB
            builder.Services.AddDatabaseConfiguration(configuration);

            // Mail
            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            // Telegram
            builder.Services.Configure<TelegramSettings>(
                builder.Configuration.GetSection("TelegramSettings"));

            // Twilio
            builder.Services.Configure<TwilioSettings>(
                builder.Configuration.GetSection("Twilio"));

            // Supabase para almacenar imágenes 
            builder.Services.Configure<SupabaseOptions>(
                builder.Configuration.GetSection("Supabase"));
            builder.Services.Configure<UploadOptions>(
                builder.Configuration.GetSection("Upload"));

            var app = builder.Build();

            app.MapHub<NotificationHub>("/hubs/notifications");

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
