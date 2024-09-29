using UFSBankingSystemWebsite.Models;

namespace UFSBankingSystemWebsite.Models.ViewModels
{
    public class CustomerViewModel
    {
        public IEnumerable<BankAccount> BankAccounts { get; set; } // User's bank accounts
        public IEnumerable<Transactions> Transactions { get; set; } // User's transactions
    }
}
