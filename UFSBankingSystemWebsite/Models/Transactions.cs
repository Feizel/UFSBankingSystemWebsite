﻿namespace UFSBankingSystem.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public int BankAccountIdSender { get; set; }
        public int BankAccountIdReceiver { get; set; }

        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string Reference { get; set; }

        public string UserEmail { get; set; }

    }

}
