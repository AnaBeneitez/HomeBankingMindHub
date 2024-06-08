using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface ITransactionsService
    {
        ResponseCollection<TransactionDTO> Get();
        ResponseModel<TransactionDTO> GetById(long id);
        Response MakeTransfer(TransferDTO transferDTO, string currentMail);
    }
}
