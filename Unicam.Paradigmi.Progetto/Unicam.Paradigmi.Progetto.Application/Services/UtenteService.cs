using System.Threading.Tasks;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Models.Entities;
using Unicam.Paradigmi.Progetto.Models.Repositories;

namespace Unicam.Paradigmi.Progetto.Application.Services
{
    /// <summary>
    /// Provides implementation for managing Utente entities.
    /// </summary>
    public class UtenteService : IUtenteService
    {
        private readonly UtenteRepository _utenteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UtenteService"/> class.
        /// </summary>
        /// <param name="utenteRepository">The repository for managing Utente entities.</param>
        public UtenteService(UtenteRepository utenteRepository)
        {
            _utenteRepository = utenteRepository;
        }

        /// <summary>
        /// Adds a new Utente entity asynchronously.
        /// </summary>
        /// <param name="utente">The Utente entity to add.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task AddUtenteAsync(Utente utente)
        {
            await _utenteRepository.AggiungiAsync(utente);
            await _utenteRepository.SaveAsync();
        }

        /// <summary>
        /// Retrieves a Utente entity by its email address asynchronously.
        /// </summary>
        /// <param name="email">The email address of the Utente to retrieve.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the Utente entity.</returns>
        public async Task<Utente> GetUtenteByEmailAsync(string email)
        {
            return await _utenteRepository.GetUtenteByEmailAsync(email);
        }

        /// <summary>
        /// Retrieves a Utente entity by its ID asynchronously.
        /// </summary>
        /// <param name="idUtente">The ID of the Utente to retrieve.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the Utente entity.</returns>
        public async Task<Utente> GetUtenteByIdAsync(int idUtente)
        {
            return await _utenteRepository.GetUtenteByIdAsync(idUtente);
        }
    }
}
