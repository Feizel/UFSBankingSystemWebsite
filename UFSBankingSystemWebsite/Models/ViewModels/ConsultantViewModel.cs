using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models.ViewModels
{
    public class ConsultantViewModel
    {
        public List<User> ClientsManaged { get; set; }  // List of clients managed by the consultant
        public List<Notification> Notifications { get; set; }  // List of notifications for the consultant
        public decimal ClientSatisfactionPercentage { get; set; } //Percentage indicating client satisafction
        public IQueryable<User> appUsers { get; set; }
        public IEnumerable<LoginSession> loginSessions { get; set; }
        public User SelectedUser { get; set; }

    }

    public class ConsultantDepositModel
    {
        public string UserEmail { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string StudentNumber { get; set; } // Nullable if not applicable

        public string EmployeeNumber { get; set; } // Nullable if not applicable

        public string IDNumber { get; set; } // Nullable if not applicable

        // Additional properties can be added as needed
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }

    //public class EditUserViewModel
    //{
    //    public string Id { get; set; }

    //    [Required]
    //    [Display(Name = "First Name")]
    //    public string FirstName { get; set; }

    //    [Required]
    //    [Display(Name = "Last Name")]
    //    public string LastName { get; set; }

    //    [EmailAddress]
    //    [Required]
    //    public string Email { get; set; }

    //    public string StudentNumber { get; set; } // Nullable if not applicable

    //    public string EmployeeNumber { get; set; } // Nullable if not applicable

    //    public string IDNumber { get; set; } // Nullable if not applicable
    //}
}