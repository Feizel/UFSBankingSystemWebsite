﻿using UFSBankingSystem.Models;

namespace UFSBankingSystem.Models.ViewModels
{
    public class CustomerViewModel
    {
        public IEnumerable<Account> BankAccounts { get; set; } // User's bank accounts
        public IEnumerable<Transaction> Transactions { get; set; } // User's transactions
    }
}
