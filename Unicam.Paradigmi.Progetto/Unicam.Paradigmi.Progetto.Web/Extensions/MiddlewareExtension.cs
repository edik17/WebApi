using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Graph.Models.ODataErrors;
using System.Net;
using Unicam.Paradigmi.Progetto.Application.Factories;

namespace Unicam.Paradigmi.Progetto.Web.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring middleware in the web application.
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// Adds the necessary middleware components to the web application.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
        /// <returns>The configured <see cref="WebApplication"/>.</returns>
        public static WebApplication? AddWebMiddleware(this WebApplication? app)
        {
            // Configure the HTTP request pipeline for development environment
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use custom JWT middleware for token validation
            app.UseMiddleware<JwtMiddleware>();

            // Enable authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controller routes
            app.MapControllers();

            return app;
        }
    }
}
