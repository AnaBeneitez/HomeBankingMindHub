using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        void Save(Account account);
    }
}
