using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories.Implementations;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;

namespace HomeBankingMindHub.Services.Implementations
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public ResponseCollection<AccountDTO> Get()
        {
            ResponseCollection<AccountDTO> response;

            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = new List<AccountDTO>();
            accountsDTO = accounts.Select(a => new AccountDTO(a)).ToList();

            return new ResponseCollection<AccountDTO>(200, "Ok", accountsDTO);
        }

        public ResponseModel<AccountDTO> GetById(long id)
        {
            ResponseModel<AccountDTO> response;

            var accountById = _accountRepository.FindById(id);

            if (accountById == null)
                return new ResponseModel<AccountDTO>(404, "No existe la cuenta con ese Id", null);

            var accountByIdDTO = new AccountDTO(accountById);

            return new ResponseModel<AccountDTO>(200, "Ok", accountByIdDTO);
        }
    }
}
