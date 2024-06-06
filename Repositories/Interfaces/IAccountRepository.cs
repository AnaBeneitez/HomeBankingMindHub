using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        Account FindByVIN(string vin);
        void Save(Account account);
        IEnumerable<Account> GetAccountsByClient(long clientId);
    }
}
