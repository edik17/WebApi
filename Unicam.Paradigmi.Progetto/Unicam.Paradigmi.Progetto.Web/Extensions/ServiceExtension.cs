﻿using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation.AspNetCore;
using Unicam.Paradigmi.Progetto.Application.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Unicam.Paradigmi.Progetto.Web.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring services.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Configures and adds web services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The service collection to which services are added.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddWebService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Progetto Tre Paradigmi",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer", //Assume che il token venga passato come Bearer token ovvero che chi presenta il token sia il legittimo proprietario
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
                        new string[] {}
                    }
                });
            });

            services.AddFluentValidationAutoValidation();

            // Read JWT authentication options from configuration
            var jwtAuthenticationOption = new JWTAuthOption();
            configuration.GetSection("JwtAuthentication").Bind(jwtAuthenticationOption);

            // Configure JWT Bearer authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    string key = jwtAuthenticationOption.Key;
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtAuthenticationOption.Issuer,
                        IssuerSigningKey = securityKey
                    };
                });

            services.Configure<JWTAuthOption>(configuration.GetSection("JwtAuthentication"));
            services.AddOptions(configuration);

            return services;
        }

        /// <summary>
        /// Configures and adds options to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The service collection to which options are added.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The updated IServiceCollection.</returns>
        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTAuthOption>(configuration.GetSection("JwtAuthentication"));
            services.Configure<EmailOption>(configuration.GetSection("EmailOption"));
            return services;
        }
    }
}
