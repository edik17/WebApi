using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unicam.Paradigmi.Progetto.Models.Context;
using Unicam.Paradigmi.Progetto.Models.Repositories;

namespace Unicam.Paradigmi.Progetto.Models.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring services related to the model layer.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds the model services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to use for configuring services.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddModelServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MydbContext>(conf =>
            {
                conf.UseSqlServer(configuration.GetConnectionString("MydbContext"));
            });

            services.AddScoped<UtenteRepository>();
            services.AddScoped<ListaDistribuzioneRepository>();
            services.AddScoped<DestinatarioRepository>();
            services.AddScoped<ListaUtenzeAssociateRepository>();

            return services;
        }
    }
}
