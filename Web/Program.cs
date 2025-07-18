
using Entity.Models.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Extensions;

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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
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


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
