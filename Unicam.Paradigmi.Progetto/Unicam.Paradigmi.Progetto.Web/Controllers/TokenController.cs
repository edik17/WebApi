using Microsoft.AspNetCore.Mvc;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Factories;
using Unicam.Paradigmi.Progetto.Application.Models.Request;

namespace Unicam.Paradigmi.Progetto.Web.Controllers
{
    /// <summary>
    /// Controller for handling token-related operations.
    /// </summary>

    // Diventa obbligatorio il routing basato sugli attributi, quindi devi specificare l'attributo [Route] per ogni controller
    // Errore 400 se il ModelState.isValid == false.   
    [ApiController] 
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUtenteService _utenteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="utenteService">The user service.</param>
        public TokenController(ITokenService tokenService, IUtenteService utenteService)
        {
            _utenteService = utenteService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Creates a new JWT token for the specified user.
        /// </summary>
        /// <param name="request">The request containing the user's email and password.</param>
        /// <returns>An <see cref="IActionResult"/> containing the generated token or an error message.</returns>
        [HttpPost]
        [Route("newToken")]
        public async Task<IActionResult> CreateTokenAsync(CreateTokenRequest request)
        {
            if (await _utenteService.GetUtenteByEmailAsync(request.Email) == null)
            {
                return BadRequest(ResponseFactory.WithError(new InvalidOperationException("email o password non valide")));
            }

            var token = await _tokenService.CreateTokenAsync(request);

            if (token == null)
            {
                return BadRequest(ResponseFactory.WithError(new InvalidOperationException("token nullo")));
            }

            return Ok(ResponseFactory.WithSuccess(token));
        }
    }
}
