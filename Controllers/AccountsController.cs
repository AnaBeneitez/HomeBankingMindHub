using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this._accountsService = accountsService;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                ResponseCollection<AccountDTO> response = _accountsService.Get();

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
                ResponseModel<AccountDTO> response = _accountsService.GetById(id);

                if(response.StatusCode != 200)
                    return StatusCode(response.StatusCode, response.Message);

                return StatusCode(response.StatusCode, response.Model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
