using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IClientsService
    {
        ResponseCollection<ClientDTO> Get();
        ResponseModel<ClientDTO> GetById(long id);
        ResponseModel<ClientDTO> GetCurrent(string email);
        ResponseModel<ClientDTO> Post(ClientRegisterDTO client);
        Response CreateAccount(string email);
        ResponseCollection<AccountClientDTO> GetAccounts(string email);
        Response CreateCard(string email, NewCardDTO newCard);
        ResponseCollection<CardDTO> GetCards(string email);
    }
}
