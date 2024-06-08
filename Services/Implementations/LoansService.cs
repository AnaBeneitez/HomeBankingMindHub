using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;

namespace HomeBankingMindHub.Services.Implementations
{
    public class LoansService : ILoansServices
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IValidationsService _validationsService;

        public LoansService(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, IClientLoanRepository clientLoanRepository, ILoanRepository loanRepository, IValidationsService validationsService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _clientLoanRepository = clientLoanRepository;
            _loanRepository = loanRepository;
            _validationsService = validationsService;
        }

        public Response GrantLoan(LoanApplicationDTO loanApplicationDTO, string currentEmail)
        {
            Loan loan = _loanRepository.FindById(loanApplicationDTO.LoanId);

            List<string> payments = new List<string>();

            if (loan != null)
                payments = loan.Payments.Split(",").ToList();

            Account toAccount = _accountRepository.FindByVIN(loanApplicationDTO.ToAccountNumber);

            Client current = _clientRepository.FindByEmail(currentEmail);

            Response response = _validationsService.GrantLoan(loanApplicationDTO, loan, payments, toAccount, current.Id);

            if(response.StatusCode == 201)
            {
                int percIncrease = 20;

                ClientLoan newClientLoan = new ClientLoan(loanApplicationDTO, current.Id, percIncrease);
                _clientLoanRepository.Save(newClientLoan);

                Transaction transaction = new Transaction(
                    TransactionType.CREDIT.ToString(), 
                    newClientLoan.Amount, 
                    $"{loan.Name} - Préstamo aprobado",
                    DateTime.Now,
                    toAccount.Id);
                _transactionRepository.Save(transaction);

                toAccount.Balance += newClientLoan.Amount;
                _accountRepository.Save(toAccount);
            }

            return response;
        }
        public ResponseCollection<LoanDTO> Get()
        {
            var loans = _loanRepository.GetAll();
            var loansDTO = new List<LoanDTO>();
            loansDTO = loans.Select(l => new LoanDTO(l)).ToList();

            ResponseCollection<LoanDTO> response = new ResponseCollection<LoanDTO>(200, "Ok", loansDTO);

            return response;
        }

    }
}
