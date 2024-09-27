namespace UFSBankingSystem.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int BankAccountID { get; set; }  // Foreign key to Account 
        public int BankAccountIdSender { get; set; }
        public int BankAccountIdReceiver { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType transactionType { get; set; }  // e.g., Deposit, Withdrawal, Transfer 
        public string Reference { get; set; }
        public string UserEmail { get; set; }
        public string Description { get; set; }  // Description of the transaction 
        public decimal BalanceAfter { get; set; }  // Balance after the transaction 
        public string Status { get; set; }  // e.g., Active, Completed, Canceled 

        // Navigation property to the associated account 
        public virtual BankAccount Account { get; set; }

    }

}
