using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Options;

public class JwtMiddleware
{
    private RequestDelegate _next;
    private readonly JWTAuthOption _jwtAuthentication;

    public JwtMiddleware(RequestDelegate next, IOptions<JWTAuthOption> _jwtAuthentication)
    {
        _next = next;
        this._jwtAuthentication = _jwtAuthentication.Value;
    }

    public async Task InvokeAsync(HttpContext context
        , IUtenteService utenteService
        , IConfiguration configuration
        , IOptions<EmailOption> emailOptions
        )
    {
        context.RequestServices.GetService<IUtenteService>();
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            attachUserToContext(context, utenteService, token);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"errore\": \"Token Mancante\"}");
            return;
        }

        await _next.Invoke(context);
    }
    
    private void attachUserToContext (HttpContext context, IUtenteService utenteService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.Key));
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _jwtAuthentication.Issuer,
                ValidAudience = _jwtAuthentication.Issuer
            }, out SecurityToken validateToken);

            var jwtToken = (JwtSecurityToken)validateToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "IdUtente").Value);
            context.Items["Utente"] = utenteService.GetUtenteByIdAsync(userId);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Errore in JwtMiddleware: " + ex.Message);
        }
    }
}