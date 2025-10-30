using Entity.DTOs.Notifications;
using Entity.DTOs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Web.Config;
using Web.Extensions;
using Web.Realtime.Hubs;


namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

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
                    Description = "Autenticaci�n JWT con esquema Bearer. **Pega solo el token (sin 'Bearer ')**.",
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

            // <summary>
            /// Configuración de fuentes de configuración en orden de prioridad.
            /// </summary>
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.AddSwaggerGen();

            // servicios y data
            builder.Services.AddProjectServices();
            builder.Services.AddCorsConfiguration(configuration);

            // Automapper
            builder.Services.AddAutoMapper(typeof(Utilities.Helper.MappingProfile));

            // JWT
            builder.Services.AddJwtAuthentication(configuration);

            // Conexi�n a DB
            builder.Services.AddDatabaseConfiguration(configuration);

            //Mail 
            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            // Telegram
            builder.Services.Configure<TelegramSettings>(
                builder.Configuration.GetSection("TelegramSettings"));

            // Twilio
            builder.Services.Configure<TwilioSettings>(
                builder.Configuration.GetSection("Twilio"));

            // Supabase para almacenar im�genes 
            builder.Services.Configure<SupabaseOptions>(builder.Configuration.GetSection("Supabase"));
            builder.Services.Configure<UploadOptions>(builder.Configuration.GetSection("Upload"));

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
