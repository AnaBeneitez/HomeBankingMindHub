using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                ResponseCollection<TransactionDTO> response = _transactionsService.Get();

                if(response.StatusCode != 200)
                {
                    return StatusCode(response.StatusCode, response.Message);
                }

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
                ResponseModel<TransactionDTO> response = _transactionsService.GetById(id);

                if (response.StatusCode != 200)
                {
                    return StatusCode(response.StatusCode, response.Message);
                }

                return StatusCode(response.StatusCode, response.Model);
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpPost]
        public IActionResult MakeTransfer([FromBody] TransferDTO transfer)
        {
            try
            {
                string email = User.FindFirst("Client") != null? User.FindFirst("Client").Value: string.Empty;

                Response response = _transactionsService.MakeTransfer(transfer, email);

                return StatusCode(response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
               return StatusCode(500, ex);
            }
        }
    }
}
