using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Options;
using Microsoft.Graph.Users.Item.SendMail;
using Unicam.Paradigmi.Progetto.Models.Entities;

namespace Unicam.Paradigmi.Progetto.Application.Services
{
    /// <summary>
    /// Provides services for sending emails using Microsoft Graph API.
    /// </summary>
    public class EmailServices : IEmailService
    {
        private readonly EmailOption _emailOption;
        private readonly IDestinatarioService _destinatarioService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServices"/> class.
        /// </summary>
        /// <param name="destinatarioService">Service for handling destinatario entities.</param>
        /// <param name="emailOption">Email options for configuring the email service.</param>
        public EmailServices(IDestinatarioService destinatarioService, IOptions<EmailOption> emailOption)
        {
            _emailOption = emailOption.Value;
            _destinatarioService = destinatarioService;
        }

        /// <summary>
        /// Sends an email with the specified subject and body to the recipients in the specified distribution list.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="idListaDestinatari">The ID of the distribution list containing the recipients.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a list of <see cref="Destinatario"/> entities who received the email.</returns>
        public async Task<List<Destinatario>> SendEmailAsync(string subject, string body, int idListaDestinatari)
        {
            var destinatariEmail = await _destinatarioService.GetDestinatariAsync(idListaDestinatari);

            List<Recipient> destinatari = new List<Recipient>();

            var clientCredential = new ClientSecretCredential(
                _emailOption.TenantId,
                _emailOption.ClientId,
                _emailOption.ClientSecret
            );

            var client = new GraphServiceClient(clientCredential);

            foreach (var destinatario in destinatariEmail)
            {
                destinatari.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = destinatario.Email
                    }
                });
            }

            Message message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = Microsoft.Graph.Models.BodyType.Text,
                    Content = body
                },
                ToRecipients = destinatari
            };

            var postRequestBody = new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            };

            await client.Users[_emailOption.From]
                .SendMail
                .PostAsync(postRequestBody);

            return destinatariEmail;
        }
    }
}
