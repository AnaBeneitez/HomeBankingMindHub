using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Tools;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(a => a.Transactions)
                .ToList();
        }

        public Account FindById(long id)
        {
            return FindByCondition(a => a.Id == id)
                .Include(a => a.Transactions)
                .FirstOrDefault();
        }

        public Account FindByVIN(string vin)
        {
            return FindByCondition(a => a.Number.ToUpper() == vin.ToUpper())
                .Include(a => a.Transactions)
                .FirstOrDefault();
        }

        public void Save(Account account)
        {
            if (account.Id == 0)
            {
                bool condition = true;
                string vinGenerate = string.Empty;

                while (condition)
                {
                    vinGenerate = RandomGenerator.GenerateVIN();
                    var dbAcc = FindByVIN(vinGenerate);
                    if (dbAcc == null) condition = false;
                }

                account.Number = vinGenerate;

                Create(account);
            }
            else
            {
                Update(account);
            }

            SaveChanges();
            RepositoryContext.ChangeTracker.Clear();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(a =>a.ClientId == clientId)
                .Include(a => a.Transactions)
                .ToList();
        }
    }
}
