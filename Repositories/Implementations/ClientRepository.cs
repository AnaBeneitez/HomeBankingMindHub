﻿using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories.Implementations
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindById(long id)
        {
            return FindByCondition(c => c.Id == id)
                .Include(c => c.Accounts)
                .Include(c => c.Cards)
                .Include(c => c.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .FirstOrDefault();
        }
        public Client FindByEmail(string email)
        {
            return FindByCondition(c => c.Email.ToUpper() == email.ToUpper())
                .Include(c => c.Accounts)
                .Include(c => c.Cards)
                .Include(c => c.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(c => c.Accounts)
                .Include(c => c.Cards)
                .Include(c => c.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
    }
}
