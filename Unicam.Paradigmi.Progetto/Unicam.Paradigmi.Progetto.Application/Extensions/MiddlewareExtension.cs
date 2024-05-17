namespace Unicam.Paradigmi.Progetto.Application.Extensions
{
    public static class MiddlewareExtension
    {
        public static WebApplication? AddApplicationMiddleware(this WebApplication? app)
        {
            app.UseMiddleware<JwtMiddleware>();
            return app;
        }
    }
}
