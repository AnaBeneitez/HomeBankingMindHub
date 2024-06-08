using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories.Interfaces;

namespace HomeBankingMindHub.Repositories.Implementations
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Loan FindById(long id)
        {
            return FindByCondition(l => l.Id == id).FirstOrDefault();
        }

        public IEnumerable<Loan> GetAll()
        {
            return FindAll().ToList();
        }
    }
}
