using FluentValidation;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Unicam.Paradigmi.Progetto.Application.Extensions
{
    /// <summary>
    /// Extension methods for adding application services to the IServiceCollection.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds application services and validators to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration instance for accessing configuration settings.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register all validators from the specified assembly.
            services.AddValidatorsFromAssembly(
                AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == "Unicam.Paradigmi.Progetto.Application")
            );

            // Register application services with their respective interfaces.

            // AddScoped registers the service with a scoped lifetime.; the service is created once per request.
            // AddTransient registers the service with a transient lifetime.; the service is created each time it is requested.
            // AddSingleton registers the service with a singleton lifetime.; the service is created once and reused for all requests.
            services.AddScoped<IUtenteService, UtenteService>();
            services.AddScoped<IListaDistribuzioneService, ListaDistribuzioneService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDestinatarioService, DestinatarioService>();
            services.AddScoped<IListaUtenzeAssociateService, ListaUtenzeAssociateService>();
            services.AddScoped<IEmailService, EmailServices>();

            return services;
        }
    }
}
