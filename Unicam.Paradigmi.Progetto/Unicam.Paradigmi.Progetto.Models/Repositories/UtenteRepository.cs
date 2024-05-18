using Microsoft.EntityFrameworkCore;
using Unicam.Paradigmi.Models.Repositories;
using Unicam.Paradigmi.Progetto.Models.Context;
using Unicam.Paradigmi.Progetto.Models.Entities;

namespace Unicam.Paradigmi.Progetto.Models.Repositories
{
    /// <summary>
    /// Repository class for performing database operations related to 'Utente' entity.
    /// </summary>
    public class UtenteRepository : GenericRepository<Utente>
    {
        protected MydbContext _ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="UtenteRepository"/> class.
        /// </summary>
        /// <param name="ctx">The database context.</param>
        public UtenteRepository(MydbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Retrieves a user by their email asynchronously.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
        public async Task<Utente> GetUtenteByEmailAsync(string email)
        {
            return await _ctx.Utenti.FirstOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// Retrieves a user by their ID asynchronously.
        /// </summary>
        /// <param name="idUtente">The ID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
        public async Task<Utente> GetUtenteByIdAsync(int idUtente)
        {
            return await _ctx.Utenti.FirstOrDefaultAsync(x => x.IdUtente == idUtente);
        }
    }
}
