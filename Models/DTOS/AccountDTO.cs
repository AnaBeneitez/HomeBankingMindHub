﻿using Newtonsoft.Json;

namespace HomeBankingMindHub.Models.DTOS
{
    public class AccountDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }

        public AccountDTO(Account account)
        {
            Id = account.Id;
            Number = account.Number;
            CreationDate = account.CreationDate;
            Balance = account.Balance;
        }
    }
}