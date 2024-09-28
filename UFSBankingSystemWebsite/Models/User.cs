using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using UFS_Banking_System_Website.Models;

namespace UFSBankingSystem.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [StringLength(20)]
        public string StudentStaffNumber { get; set; }
        [StringLength(13)]
        public string IDnumber { get; set; }
        public string UserRole { get; set; }
        public string AccountNumber { get; set; }

        // Navigation properties
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual Consultant Consultant { get; set; }
        public virtual FinancialAdvisor FinancialAdvisor { get; set; }
        public bool IsConsultant { get; set; }
        public bool IsCustomer { get; set; }
    }
}
