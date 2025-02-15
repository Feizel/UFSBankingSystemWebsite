﻿using UFSBankingSystemWebsite.Models;

namespace UFSBankingSystemWebsite.Models.ViewModels
{
    public class FinancialAdvisorViewModel
    {
        public User CurrentUser { get; set; }
        public List<Transactions> Transactions { get; set; }
        public BankAccount CurrentUserBankAccount { get; set; }
    }

    public class AdvisorViewModel : FinancialAdvisorViewModel
    {
        public string UserEmail { get; set; }
        public string Advise { get; set; }
    }
}
