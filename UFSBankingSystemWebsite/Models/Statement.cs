using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystem.Models
{
    public class Statement
    {
        [Key]
        public int StatementID { get; set; } // Primary key

        [Required]
        public string Id { get; set; } // Foreign key to User

        [Required]
        public DateTime Date { get; set; } // Date of the statement

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; } // Amount in the statement

        [Required]
        [StringLength(500)]
        public string Description { get; set; } // Description of the statement

        // Navigation property (if needed)
        public virtual User User { get; set; }
    }
}