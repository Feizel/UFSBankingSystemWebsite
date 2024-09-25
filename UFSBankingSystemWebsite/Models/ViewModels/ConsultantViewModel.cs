using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystem.Models.ViewModels
{
    public class ConsultantViewModel
    {
        public List<User> ClientsManaged { get; set; }  // List of clients managed by the consultant
        public List<Notification> Notifications { get; set; }  // List of notifications for the consultant
        public decimal ClientSatisfactionPercentage { get; set; } //Percentage indicating client satisafction
        public IQueryable<User> appUsers { get; set; }
        public IEnumerable<LoginSessions> loginSessions { get; set; }
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

        public int? StudentNumber { get; set; } // Nullable if not applicable

        public int? EmployeeNumber { get; set; } // Nullable if not applicable

        public long? IDNumber { get; set; } // Nullable if not applicable
    }
}