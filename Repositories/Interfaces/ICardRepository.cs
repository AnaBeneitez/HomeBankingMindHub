using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card FindById(long id);
        Card FindByNumber(string number);
        void Save(Card card);
        IEnumerable<Card> GetCardsByClient(long clientId);
    }
}
