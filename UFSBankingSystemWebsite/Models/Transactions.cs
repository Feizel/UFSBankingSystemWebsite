using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionID { get; set; }
        public string Id { get; set; }  // Foreign key of the Id of the User
        [Required]
        public int BankAccountID { get; set; }
        public int BankAccountIdSender { get; set; }
        public int BankAccountIdReceiver { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }
        public string Status { get; set; }
        public string UserEmail { get; set; }

        // Navigation properties
        public virtual BankAccount BankAccount { get; set; }
        public virtual User User { get; set; }

    }

}
