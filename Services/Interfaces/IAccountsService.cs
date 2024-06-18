using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IAccountsService
    {
        ResponseCollection<AccountDTO> Get();
        ResponseModel<AccountDTO> GetById(long id);
    }
}
