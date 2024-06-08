using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;

namespace HomeBankingMindHub.Services.Implementations
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IValidationsService _validationsService;
        public TransactionsService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, IClientRepository clientRepository, IValidationsService validationsService)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _validationsService = validationsService;
        }
        public ResponseCollection<TransactionDTO> Get()
        {
            ResponseCollection<TransactionDTO> response;

            var transactions = _transactionRepository.GetAllTransactions();
            var transactionsDTO = new List<TransactionDTO>();
            transactionsDTO = transactions.Select(t => new TransactionDTO(t)).ToList();

            response = new ResponseCollection<TransactionDTO>(200, "Ok", transactionsDTO);
            return response;
        }

        public ResponseModel<TransactionDTO> GetById(long id)
        {
            ResponseModel<TransactionDTO> response;

            var transactionById = _transactionRepository.FindTransactionById(id);

            if (transactionById == null)
            {
                response = new ResponseModel<TransactionDTO>(403, "La transacción no fue encontrada", null);
                return response;
            }

            var transactionByIdDTO = new TransactionDTO(transactionById);
            
            response = new ResponseModel<TransactionDTO>(200, "Ok", transactionByIdDTO);
            
            return response;

        }

        public Response MakeTransfer(TransferDTO transferDTO, string currentMail)
        {
            Client current = _clientRepository.FindByEmail(currentMail);

            Response response;
           
            if (current == null)
            {
                response = new Response(403, "Cliente no encontrado");
            }

            Account from = _accountRepository.FindByVIN(transferDTO.FromAccountNumber.ToUpper());
            Account to = _accountRepository.FindByVIN(transferDTO.ToAccountNumber.ToUpper());

            response = _validationsService.MakeTransfer(transferDTO, from, to, current.Id);

            if (response.StatusCode == 201)
            {
                Transaction fromTransaction = new Transaction(
                    TransactionType.DEBIT.ToString(), 
                    (transferDTO.Amount * -1), 
                    $"{transferDTO.Description} - Enviado a cta {to.Number}", 
                    DateTime.Now, 
                    from.Id);
                Transaction toTransaction = new Transaction(
                    TransactionType.CREDIT.ToString(), 
                    transferDTO.Amount,
                    $"{transferDTO.Description} - Recibido de cta {from.Number}",
                    DateTime.Now,
                    to.Id);

                _transactionRepository.Save(fromTransaction);
                _transactionRepository.Save(toTransaction);

                from.Balance -= transferDTO.Amount;
                to.Balance += transferDTO.Amount;

                _accountRepository.Save(from);
                _accountRepository.Save(to);
            }

            return response;
        }
    }
}
