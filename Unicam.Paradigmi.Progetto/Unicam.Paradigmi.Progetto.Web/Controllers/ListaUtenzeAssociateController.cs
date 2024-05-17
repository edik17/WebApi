﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicam.Paradigmi.Progetto.Application.Abstractions.Services;
using Unicam.Paradigmi.Progetto.Application.Factories;
using Unicam.Paradigmi.Progetto.Application.Models.Dtos;
using Unicam.Paradigmi.Progetto.Application.Models.Request;
using Unicam.Paradigmi.Progetto.Application.Models.Responses;

namespace Unicam.Paradigmi.Progetto.Web.Controllers
{
    /*
     * this class is a controller that manages the requests related to the list of users associated with the list
     * 
     * @param _listaUtenzeAssociateService: service that manages the list of users associated with the list
     * @param _listaDistribuzioneService: service that manages the distribution list
     * return: the response to the request
     * **/
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ListaUtenzeAssociateController : ControllerBase
    {
       private readonly IListaUtenzeAssociateService _listaUtenzeAssociateService;
        private readonly IListaDistribuzioneService _listaDistribuzioneService;

        public ListaUtenzeAssociateController (IListaUtenzeAssociateService listaUtenzeAssociateService, IListaDistribuzioneService listaDistribuzioneService)
        {
            this._listaUtenzeAssociateService = listaUtenzeAssociateService;
            this._listaDistribuzioneService = listaDistribuzioneService;
        }

        /*
         * this class manages the request to add a new user to the list of users associated with the list.       
         * We check if the user exists, if it exists we add it to the list of users associated with the list and return the response with the user added
         * We Use async to make the method asynchronous, so that it can be executed in parallel with other methods
         *
         * @param addDestinatariorequest: request to add a new user to the list of users associated with the list
         * return the response to the request
         **/

        [HttpPost]
        [Route("newDestinatario")]
        public async Task<IActionResult> AddDestinatarioAsync(AddDestinatarioRequest addDestinatariorequest)
        {
            var idUtente = (int)HttpContext.Items["IdUtente"];

            var idProprietario = await _listaDistribuzioneService.GetidProprietarioAsync(addDestinatariorequest.IdListaDistribuzione);
            if (idProprietario.Equals(idUtente))
            {
                var aggiunto = await _listaUtenzeAssociateService.AddDestinatarioAsync(addDestinatariorequest.IdListaDistribuzione, addDestinatariorequest.Email);
                if (aggiunto == null)
                {
                    return BadRequest(ResponseFactory.WithError("non esiste l'utente da aggiungere"));
                }
            

                var response = new AddDestinatarioResponse()
                {
                    Destinatario = new DestinatarioDto(aggiunto)
                   
                };
                return Ok(ResponseFactory.WithSuccess(response));
            }
          return BadRequest(ResponseFactory.WithError("non sei proprietario della lista")); 
                
        }

        /*
         * this class manages the request to delete a user from the list of users associated with the list.
         * We check if the user exists, if it exists we delete it from the list of users associated with the list and return the response with the user deleted
         * 
         * @param deleteDestinatarioRequest: request to delete a user from the list of users associated with the list
         * return the response to the request
         * **/

        [HttpDelete]
        [Route("deleteDestinatario")]
        public async Task<IActionResult> DeleteDestinatarioAsync(DeleteDestinatarioRequest deleteDestinatarioRequest)
        {
            var idUtente = (int)HttpContext.Items["IdUtente"];
            var idProprietario = await _listaDistribuzioneService.GetidProprietarioAsync(deleteDestinatarioRequest.IdLista);
            if (idProprietario.Equals(idUtente))
            {
                var rimosso = await _listaUtenzeAssociateService.DeleteDestinatarioAsync(deleteDestinatarioRequest.NomeLista, deleteDestinatarioRequest.Email);
                if (rimosso == false)
                {
                    return BadRequest(ResponseFactory.WithError("dati inseriti non validi"));
                }
            }
            else
            {
                return BadRequest(ResponseFactory.WithError("non sei proprietario della lista"));
            }

            return Ok();
        }

    }
}

