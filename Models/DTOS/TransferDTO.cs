﻿namespace HomeBankingMindHub.Models.DTOS
{
    public class TransferDTO
    {
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

    }
}
