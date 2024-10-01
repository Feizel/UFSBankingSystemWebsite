using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UFSBankingSystemWebsite.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountID { get; set; }
        [Required]
        public string Id { get; set; } // Foreign key to User
        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        [Required]
        public string BankAccountType { get; set; }
        public int AccountOrder { get; set; }
        public string UserEmail { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }

}
