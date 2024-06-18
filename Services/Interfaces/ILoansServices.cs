using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface ILoansServices
    {
        Response GrantLoan(LoanApplicationDTO loanApplicationDTO, string currentEmail);
        ResponseCollection<LoanDTO> Get();
    }
}
