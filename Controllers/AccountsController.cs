using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var accountsDTO = new List<AccountDTO>();
                accountsDTO = accounts.Select(a => new AccountDTO(a)).ToList();

                return Ok(accountsDTO);
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
                var accountById = _accountRepository.FindById(id);

                if (accountById == null)
                {
                    return Forbid();
                }

                var accountByIdDTO = new AccountDTO(accountById);

                return Ok(accountByIdDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
