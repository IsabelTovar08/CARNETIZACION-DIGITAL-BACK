﻿using Entity.DTOs.Notifications;
using Entity.DTOs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Web.Extensions;
using Web.Realtime.Hubs;

// 👇 agregado para configurar QuestPDF
using QuestPDF.Infrastructure;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 👇 licencia gratuita de QuestPDF (Community)
            QuestPDF.Settings.License = LicenseType.Community;

            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Swagger + JWT Bearer
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

                // Definir el esquema Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticación JWT con esquema Bearer. **Pega solo el token (sin 'Bearer ')**.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // Requisito global de seguridad
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

            builder.Services.AddSwaggerGen();

            // servicios y data
            builder.Services.AddProjectServices();

            //Cors
            builder.Services.AddCorsConfiguration(configuration);

            //Automapper
            builder.Services.AddAutoMapper(typeof(Utilities.Helper.MappingProfile));

            // JWT
            builder.Services.AddJwtAuthentication(configuration);
            //Conexión 
            builder.Services.AddDatabaseConfiguration(configuration);

            //Mail 
            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            //Telegram 
            builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("TelegramSettings"));

            builder.Services.Configure<TwilioSettings>(
                builder.Configuration.GetSection("Twilio"));

            // Supabase para almacenar imágenes 
            builder.Services.Configure<SupabaseOptions>(builder.Configuration.GetSection("Supabase"));
            builder.Services.Configure<UploadOptions>(builder.Configuration.GetSection("Upload"));

            var app = builder.Build();

            app.MapHub<NotificationHub>("/hubs/notifications");

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
