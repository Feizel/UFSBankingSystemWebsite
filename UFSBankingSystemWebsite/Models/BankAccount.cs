using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UFSBankingSystem.Models
{
    public class BankAccount
    {
        public int BankAccountID { get; set; }
        public string Id { get; set; } // Foreign key to User
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string BankAccountType { get; set; }
        public string AccountName { get; set; } // Account name
        public int AccountOrder { get; set; } // control if main account or savings

        public string UserEmail { get; set; }

        //public ICollection<Transaction> Transactions { get; set; }

        // Navigation property to the associated user
        public virtual User User { get; set; }

        // Navigation property to the associated transactions
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
