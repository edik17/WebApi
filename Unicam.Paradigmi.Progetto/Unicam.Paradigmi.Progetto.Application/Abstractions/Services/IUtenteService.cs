using Unicam.Paradigmi.Progetto.Models.Entities;
using System.Threading.Tasks;

namespace Unicam.Paradigmi.Progetto.Application.Abstractions.Services
{
    /// <summary>
    /// Provides operations for managing Utente entities.
    /// </summary>
    public interface IUtenteService
    {
        /// <summary>
        /// Adds a new Utente entity asynchronously.
        /// </summary>
        /// <param name="utente">The Utente entity to add.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task AddUtenteAsync(Utente utente);

        /// <summary>
        /// Retrieves a Utente entity by its email address asynchronously.
        /// </summary>
        /// <param name="email">The email address of the Utente to retrieve.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the Utente entity.</returns>
        public Task<Utente> GetUtenteByEmailAsync(string email);

        /// <summary>
        /// Retrieves a Utente entity by its ID asynchronously.
        /// </summary>
        /// <param name="idUtente">The ID of the Utente to retrieve.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the Utente entity.</returns>
        public Task<Utente> GetUtenteByIdAsync(int idUtente);
    }
}
