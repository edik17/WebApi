using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Models.Request;
using Unicam.Paradigmi.Progetto.Application.Options;
using Unicam.Paradigmi.Progetto.Models.Entities;

namespace Unicam.Paradigmi.Progetto.Application.Services
{
    /// <summary>
    /// Provides methods for creating JWT tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JWTAuthOption _jwtAuthOption;
        private readonly IUtenteService _utenteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="jwtAuthOption">The JWT authentication options.</param>
        /// <param name="utenteService">The user service.</param>
        public TokenService(IOptions<JWTAuthOption> jwtAuthOption, IUtenteService utenteService)
        {
            _jwtAuthOption = jwtAuthOption.Value;
            _utenteService = utenteService;
        }

        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="request">The request containing the user's email and password.</param>
        /// <returns>A JWT token as a string.</returns>
        public async Task<string> CreateTokenAsync(CreateTokenRequest request)
        {
            var utente = await _utenteService.GetUtenteByEmailAsync(request.Email);

            if (utente == null)
            {
                throw new ArgumentException("Invalid email or password.");
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("Email", utente.Email),
                new Claim("IdUtente", utente.IdUtente.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOption.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                _jwtAuthOption.Issuer,
                null,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
