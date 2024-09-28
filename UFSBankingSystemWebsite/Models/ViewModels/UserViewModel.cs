using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystem.Models.ViewModels.Admin
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public User AppUser { get; set; }
        public BankAccount BankAccount { get; set; }
        public string AccountNumber { get; set; }
        public string IDNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string fullName => $"{FirstName} {LastName}";
        public decimal Balance { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserPageViewModel 
    {
        public List<UserViewModel> AppUsers { get; set; }
    }

    // Profile Management Model
    public class UserProfileModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string IDNumber { get; set; }
    }

    // Change Password Model
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords must match.")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
