using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UFSBankingSystem.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountID { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        [Required]
        public string BankAccountType { get; set; }
        [Required]
        public string AccountName { get; set; }
        public int AccountOrder { get; set; }
        public string UserEmail { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

}
