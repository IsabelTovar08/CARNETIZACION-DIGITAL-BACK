namespace Web.Extensions
{
    public static class ServiceExtensionsCors
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var origenesPermitidos = configuration.GetSection("OrigenesPermitidos").Get<string[]>();

            services.AddCors(opciones =>
            {
                opciones.AddPolicy("AllowFrontend", politica =>
                {
                    politica.WithOrigins(origenesPermitidos)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                });
            });

            return services;
        }
    }
}
