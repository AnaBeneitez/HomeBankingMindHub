using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories.Implementations
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Transaction FindTransactionById(long id)
        {
            return this.FindByCondition(a => a.Id == id).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return this.FindAll().ToList();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
    }
}
