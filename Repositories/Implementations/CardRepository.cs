using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Tools;

namespace HomeBankingMindHub.Repositories.Implementations
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Card FindById(long id)
        {
            return this.FindByCondition(c => c.Id == id)
                .FirstOrDefault();
        }

        public Card FindByNumber(string number)
        {
            return this.FindByCondition(c => c.Number == number)
                .FirstOrDefault();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return this.FindAll().ToList();
        }

        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return this.FindByCondition(c => c.ClientId == clientId)
                .ToList();
        }

        public void Save(Card card)
        {
            if (card.Id == 0)
            {
                bool condition = true;
                string cardNumberGenerate = string.Empty;

                while (condition)
                {
                    cardNumberGenerate = RandomGenerator.GenerateCardNumber();
                    var dbCard = FindByNumber(cardNumberGenerate);
                    if(dbCard == null) condition = false;
                }

                card.Number = cardNumberGenerate;
                card.Cvv = RandomGenerator.GenerateCVV();

                Create(card);

            }
            else
            {
                Update(card);
            }

            SaveChanges();
        }
    }
}
