using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Models.DTOS
{
    public class ClientLoanDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }

        public ClientLoanDTO(ClientLoan clientLoan)
        {
            Id = clientLoan.Id;
            LoanId = clientLoan.LoanId;
            Name = clientLoan.Loan.Name;
            Amount = clientLoan.Amount;
            Payments = Convert.ToInt32(clientLoan.Payments);
        }
    }
}
