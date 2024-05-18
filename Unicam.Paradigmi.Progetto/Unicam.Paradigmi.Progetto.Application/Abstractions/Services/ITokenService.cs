using Unicam.Paradigmi.Progetto.Application.Models.Request;

namespace Unicam.Paradigmi.Progetto.Application.Abstractions.Services
{
    /// <summary>
    /// Interface for the token service, providing methods to create JWT tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token based on the provided request.
        /// </summary>
        /// <param name="request">The request containing user credentials.</param>
        /// <returns>A task that represents the asynchronous operation, containing the JWT token as a string.</returns>
        Task<string> CreateTokenAsync(CreateTokenRequest request);
    }
}
