
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models.ViewModels

{
    public class BankAccountViewModel
    {
        public BankAccount BankAccount { get; set; }
        public IEnumerable<Transactions> Transactions { get; set; }
    }

    public class AccountOverviewViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public List<Transactions> Transactions { get; set; }
    }

    public class CreateBankAccountViewModel
    {
        public string Id { get; set; } // Foreign key to User
        [Required]
        public string AccountType { get; set; } // e.g., Savings, Checking

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Initial deposit must be a positive amount.")]
        public decimal InitialDeposit { get; set; }
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
    //public class MoneyTransferViewModel
    //{
    //    [Required]
    //    public int SenderBankAccountId { get; set; }

      
    //    public int ReceiverBankAccountId { get; set; }

    //    [Required(ErrorMessage = "The amount is required.")]
    //    [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than zero.")]
    //    public decimal Amount { get; set; }

       
    //    public string SenderBankAccountNumber { get; set; }

    //    [Required(ErrorMessage = "The receiver's bank account number is required.")]
      
    //    public string ReceiverBankAccountNumber { get; set; }
      
    //    public decimal AvailableBalance { get; set; }
    //}

    public class WithdrawViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bank Account ID is required.")]
        public int BankAccountId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }

    public class WithdrawSuccessViewModel
    {
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public DateTime WithdrawDate { get; set; }
    }

    public class MoneyTransferViewModel
    {
        [Required(ErrorMessage = "Sender's Bank Account ID is required.")]
        public int SenderBankAccountId { get; set; }

        [Required(ErrorMessage = "Receiver's Bank Account ID is required.")]
        public int ReceiverBankAccountId { get; set; }

        [Required(ErrorMessage = "The amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
    public class TransferSuccessViewModel
    {
        public decimal Amount { get; set; }
        public string ReceiverAccount { get; set; }
        public string SenderAccount { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
