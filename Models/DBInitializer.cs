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
        }
    }
}
