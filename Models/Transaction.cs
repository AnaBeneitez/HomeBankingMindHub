using HomeBankingMindHub.Models.Enums;

namespace HomeBankingMindHub.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Account Account { get; set; }
        public long AccountId { get; set; }
        public Transaction(string type, double amount, string description, DateTime date, long accountID)
        {
            Type = type;
            Amount = amount;
            Description = description;
            Date = date;
            AccountId = accountID;
        }
        public Transaction()
        {
            
        }

    }
}
