namespace Web.Extensions
{
    public static class ServiceExtensionsCors
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var OrigenesPermitidos = configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");

            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(politica =>
                {
                    politica.WithOrigins(OrigenesPermitidos).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            return services;
        }
    }
}
