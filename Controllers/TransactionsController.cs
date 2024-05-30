using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            this._transactionRepository = transactionRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var transactions = _transactionRepository.GetAllTransactions();
                var transactionsDTO = new List<TransactionDTO>();
                transactionsDTO = transactions.Select(t => new TransactionDTO(t)).ToList();

                return Ok(transactionsDTO);
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
                var transactionById = _transactionRepository.FindTransactionById(id);

                if (transactionById == null)
                {
                    return Forbid();
                }
                var transactionByIdDTO = new TransactionDTO(transactionById);

                return Ok(transactionByIdDTO);
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, ex.Message); 
            }
        }
    }
}
