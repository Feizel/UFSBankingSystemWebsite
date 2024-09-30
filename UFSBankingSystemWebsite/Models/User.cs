using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using UFSBankingSystemWebsite.Models;

namespace UFSBankingSystemWebsite.Models
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
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual Consultant Consultants { get; set; }
        public virtual FinancialAdvisor FinancialAdvisors { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; } = new List<Transactions>();
        public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();
        public bool IsConsultant { get; set; }
        public bool IsCustomer { get; set; }
    }
}
