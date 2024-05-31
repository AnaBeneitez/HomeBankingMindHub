using HomeBankingMindHub.Models.Enums;

namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client {FirstName="Ana", LastName="Beneitez", Email="ana@gmail.com", Password="123456"},
                    new Client{FirstName="Emiliano", LastName="Romay", Email="emi@gmail.com", Password="123456"},
                    new Client{FirstName="Juan", LastName="Perez", Email="juan@gmail.com", Password="123456"}
                };

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var clientAna = context.Clients.FirstOrDefault(c => c.Email == "ana@gmail.com");
                if (clientAna != null)
                {
                    var accountsAna = new Account[] {
                        new Account{ClientId = clientAna.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 100000 },
                        new Account{ClientId = clientAna.Id, CreationDate= DateTime.Now, Number ="VIN002", Balance = 10 }
                    };

                    context.Accounts.AddRange(accountsAna);
                    context.SaveChanges();
                }
            }

            if (!context.Transactions.Any())
            {
                var accountVIN001 = context.Accounts.FirstOrDefault(a => a.Number == "VIN001");
                if (accountVIN001 != null)
                {
                    var transactionsVIN001 = new Transaction[]
                    {
                        new Transaction { AccountId = accountVIN001.Id, Type = Enums.TransactionType.CREDIT.ToString(),
                            Amount = 20000, Description = "Acreditacion de haberes", Date = DateTime.Now.AddHours(-10)},
                        new Transaction { AccountId = accountVIN001.Id, Type = Enums.TransactionType.DEBIT.ToString(),
                            Amount = 5000, Description = "Verduleria Los mellis - Compra", Date = DateTime.Now.AddHours(-6)},
                        new Transaction { AccountId = accountVIN001.Id, Type = Enums.TransactionType.DEBIT.ToString(),
                            Amount = 10000, Description = "Almacen Francesca - Compra", Date = DateTime.Now.AddHours(-2)}
                    };
                    context.Transactions.AddRange(transactionsVIN001);
                    context.SaveChanges();
                }
            }

            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                {
                    new Loan{Name= "Hipotecario", MaxAmount = 5000000, Payments= "12,24,36,48,60"},
                    new Loan{Name= "Refacción", MaxAmount = 3000000, Payments= "12,24,36,48"},
                    new Loan{Name= "Automotor", MaxAmount = 1000000, Payments= "12,24,36"},
                    new Loan{Name= "Personal", MaxAmount = 500000, Payments= "6,12,24"}
                };

                context.Loans.AddRange(loans);
                context.SaveChanges();

                var clientAna = context.Clients.FirstOrDefault(c => c.Email == "ana@gmail.com");
               
                if(clientAna != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                   
                    if (loan1 != null)
                    {
                        var anaLoans1 = new ClientLoan {Amount = 4500000, ClientId = clientAna.Id, LoanId = loan1.Id, Payments = "48"};

                        context.ClientLoans.Add(anaLoans1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Automotor");

                    if (loan2 != null)
                    {
                        var anaLoans2 = new ClientLoan { Amount = 600000, ClientId = clientAna.Id, LoanId = loan2.Id, Payments = "24"};

                        context.ClientLoans.Add(anaLoans2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Personal");

                    if (loan3 != null)
                    {
                        var analoan3 = new ClientLoan { Amount = 200000, ClientId = clientAna.Id, LoanId = loan3.Id, Payments = "6" };

                        context.ClientLoans.Add(analoan3);
                    }
                    context.SaveChanges();
                }
            }

            if (!context.Cards.Any())
            {
                var clientAna = context.Clients.FirstOrDefault(c => c.Email == "ana@gmail.com");

                if (clientAna != null)
                {
                    var cards = new Card[]
                    {
                        new Card
                        {
                            ClientId= clientAna.Id,
                            CardHolder = $"{clientAna.FirstName} {clientAna.LastName}",
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3189-5896-1432-7485",
                            Cvv = 690,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(6),
                        },
                        new Card
                        {
                            ClientId= clientAna.Id,
                            CardHolder = $"{clientAna.FirstName} {clientAna.LastName}",
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "1234-5678-9123-4567",
                            Cvv = 526,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(6),
                        }
                    };

                    context.AddRange(cards);
                    context.SaveChanges();
                }
            }
        }
    }
}
