using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            this._clientsService = clientsService;
        }

        [HttpGet]
        [Authorize (Policy = "AdminOnly")]
        public IActionResult Get()
        {
            try
            {
                ResponseCollection<ClientDTO> response = _clientsService.Get();
    
                return StatusCode(response.StatusCode, response.Collection);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                ResponseModel<ClientDTO> response = _clientsService.GetById(id);

                if (response.StatusCode != 200)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]

        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null? User.FindFirst("CLient").Value : string.Empty;

                ResponseModel<ClientDTO> response = _clientsService.GetCurrent(email);

                if (response.StatusCode != 200)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Model);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientRegisterDTO client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(400, "datos inválidos");

                ResponseModel<ClientDTO> response = _clientsService.Post(client);

                if (response.StatusCode != 201)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Model);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateAccount()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("CLient").Value : string.Empty;

                ResponseModel<ClientDTO> response = _clientsService.CreateAccount(email);

                if (response.StatusCode != 201)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Model);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("CLient").Value : string.Empty;

                ResponseCollection<AccountClientDTO> response = _clientsService.GetAccounts(email);

                if (response.StatusCode != 200)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Collection);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateCard([FromBody] NewCardDTO newCard)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("CLient").Value : string.Empty;

                Response response = _clientsService.CreateCard(email, newCard);

                return StatusCode(response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCards()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("CLient").Value : string.Empty;

                ResponseCollection<CardDTO> response = _clientsService.GetCards(email);

                if (response.StatusCode != 200)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Collection);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
