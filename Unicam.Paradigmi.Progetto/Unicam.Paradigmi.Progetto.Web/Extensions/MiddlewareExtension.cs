﻿

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Graph.Models.ODataErrors;
using System.Net;
using Unicam.Paradigmi.Progetto.Application.Factories;

namespace Unicam.Paradigmi.Progetto.Web.Extensions
{
    public static class MiddlewareExtension
    {
        public static WebApplication? AddWebMiddleware(this WebApplication? app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.MapControllers();
            return app;
        }
    }
}
