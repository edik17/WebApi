using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Options;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JWTAuthOption _jwtAuthentication;

    public JwtMiddleware(RequestDelegate next, IOptions<JWTAuthOption> jwtAuthentication)
    {
        _next = next;
        _jwtAuthentication = jwtAuthentication.Value;
    }

    /// <summary>
    /// Middleware to validate JWT tokens and attach user information to the context.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IUtenteService utenteService)
    {
        // Retrieve the JWT token from the request header
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            // Validate and attach user information to the context
            AttachUserToContext(context, utenteService, token);
        }
        else
        {
            // If token is missing, return Unauthorized status
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\": \"Missing Token\"}");
            return;
        }

        // Continue to the next middleware
        await _next.Invoke(context);
    }

    /// <summary>
    /// Validates the JWT token and attaches user information to the context.
    /// </summary>
    private void AttachUserToContext(HttpContext context, IUtenteService utenteService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.Key));

            // Validate the JWT token
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

            // Attach user information to the context
            context.Items["Utente"] = utenteService.GetUtenteByIdAsync(userId);
        }
        catch (Exception ex)
        {
            // Log and handle token validation errors
            Console.WriteLine("Error in JwtMiddleware: " + ex.Message);
            throw; // Rethrow the exception for centralized error handling
        }
    }
}
