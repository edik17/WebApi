using System.Collections.Generic;
using System.Threading.Tasks;
using Unicam.Paradigmi.Progetto.Models.Entities;

namespace Unicam.Paradigmi.Progetto.Application.Abstractions.Services
{
    /// <summary>
    /// Defines the contract for an email service that handles sending emails to a list of recipients.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email with the specified subject and body to the recipients in the specified distribution list.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="idListaDestinatari">The ID of the distribution list containing the recipients.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a list of <see cref="Destinatario"/> entities who received the email.</returns>
        public Task<List<Destinatario>> SendEmailAsync(string subject, string body, int idListaDestinatari);
    }
}
