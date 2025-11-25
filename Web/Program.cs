using Entity.Context;
using Entity.DTOs.Notifications;
using Entity.DTOs.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Utilities.Notifications.Implementations;
using Utilities.Notifications.Interfases;
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

            QuestPDF.Settings.License = LicenseType.Community;

            // CONFIGURAR LOGGING EXPLÍCITO PARA CONSOLE Y DEBUG
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Ajustar niveles para ver más detalle
            builder.Logging.SetMinimumLevel(LogLevel.Information);

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

            //builder.Services.AddSwaggerGen();

            // servicios y data
            builder.Services.AddProjectServices();
            builder.Services.AddCorsConfiguration(configuration);

            //Servicio en segundo plano para auto-finalizar eventos
            builder.Services.AddHostedService<Business.Services.Events.EventAutoFinalizerService>();

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

            builder.Services.AddSingleton(provider =>
               provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<EmailSettings>>().Value);


            // Supabase para almacenar im�genes 
            builder.Services.Configure<SupabaseOptions>(builder.Configuration.GetSection("Supabase"));
            builder.Services.Configure<UploadOptions>(builder.Configuration.GetSection("Upload"));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                try
                {
                    Console.WriteLine("🏗️ Verificando base de datos...");

                    // 🔍 Detectar si existe la tabla de migraciones
                    var hasHistoryTable = db.Database.ExecuteSqlRaw(
                        "SELECT COUNT(*) FROM pg_tables WHERE tablename='__EFMigrationsHistory';"
                    );

                    // ⚙️ Si no hay migraciones creadas, genera y aplica una inicial automáticamente
                    if (hasHistoryTable == 0)
                    {
                        Console.WriteLine("⚙️ No hay migraciones, creando AutoInitial...");
                        RunMigrationCommand("dotnet ef migrations add AutoInitial --project Entity --startup-project Web");
                    }

                    try
                    {
                        Console.WriteLine("📦 Aplicando migraciones pendientes...");
                        db.Database.Migrate();
                        Console.WriteLine("✅ Migraciones aplicadas correctamente.");
                    }
                    catch (Exception ex) when (ex.Message.Contains("PendingModelChangesWarning"))
                    {
                        Console.WriteLine("⚠️ Se detectaron cambios pendientes en el modelo. Creando migración AutoFix...");
                        RunMigrationCommand("dotnet ef migrations add AutoFix --project Entity --startup-project Web");
                        db.Database.Migrate();
                        Console.WriteLine("✅ Migraciones sincronizadas automáticamente.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error inicializando base de datos: {ex.Message}");
                }
            }

            //  Función auxiliar para ejecutar comandos dotnet dentro del contenedor
            static void RunMigrationCommand(string command)
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "bash",
                        Arguments = $"-c \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(output);
                if (!string.IsNullOrWhiteSpace(error))
                    Console.WriteLine($"⚠️ EF CLI output: {error}");
            }


            app.MapHub<NotificationHub>("/hubs/notifications");
            app.MapHub<AttendanceHub>("/hubs/attendance");

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting(); 
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
