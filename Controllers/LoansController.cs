using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoansServices _loansServices;

        public LoansController(ILoansServices loansServices)
        {
            this._loansServices = loansServices;
        }

        [HttpPost]
        public IActionResult GrantLoan([FromBody] LoanApplicationDTO loanApplication)
        {
            try
            {
                string email = User.FindFirst("Client") != null? User.FindFirst("Client").Value : string.Empty;

                Response response = _loansServices.GrantLoan(loanApplication, email);

                return StatusCode(response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                ResponseCollection<LoanDTO> response = _loansServices.Get();

                return StatusCode(response.StatusCode, response.Collection);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
