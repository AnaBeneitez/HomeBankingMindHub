using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IValidationsService
    {
        Response MakeTransfer(TransferDTO transferDTO, Account fromAccount, Account toAccount, long currentId);
    }
}
