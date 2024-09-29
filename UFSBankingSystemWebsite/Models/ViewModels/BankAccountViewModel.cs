
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models.ViewModels

{
    public class BankAccountViewModel
    {
        public BankAccount BankAccount { get; set; }
        public IEnumerable<Transactions> Transactions { get; set; }
    }
    public class CreateAccountViewModel
    {
        [Required]
        public string AccountType { get; set; } // e.g., Savings, Checking

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Initial deposit must be a positive amount.")]
        public decimal InitialDeposit { get; set; }
        [Required(ErrorMessage = "Please provide an account name.")]
        public string AccountName { get; set; } // Account name
    }
    public class CashSentViewModel
    {
        public int BankAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal AvailableBalance { get; set; }  // Add this property
    }
    public class BankAccountViewModela
    {
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string BankAccountType { get; set; }
    }
    public class MoneyTransferViewModel
    {
        [Required]
        public int SenderBankAccountId { get; set; }

      
        public int ReceiverBankAccountId { get; set; }

        [Required(ErrorMessage = "The amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than zero.")]
        public decimal Amount { get; set; }

       
        public string SenderBankAccountNumber { get; set; }

        [Required(ErrorMessage = "The receiver's bank account number is required.")]
      
        public string ReceiverBankAccountNumber { get; set; }
      
        public decimal AvailableBalance { get; set; }
    }


}
