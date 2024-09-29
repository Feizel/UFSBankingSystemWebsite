using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Address Does Not Exist")]
        [UIHint("email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Incorrect Password")]
        [DataType(DataType.Password)]
        [UIHint("password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }

    public class RegisterViewModel
    {
        public string RegisterAs { get; set; } = "User";

        //[Required(ErrorMessage = "Role is required.")]
        //public required string UserRole { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [DisplayName("Last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a unique email address")]
        [DisplayName("Email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter ID or passport number.")]
        [DisplayName("ID or Passport number")]
        public long IdPassportNumber { get; set; }

        [DisplayName("Student or Staff number")]
        public int StudentStaffNumber { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateProfileViewModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AccountNumber { get; set; }
        public string IDNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Userrole { get; set; }
    }

    public class ConsultantUpdateUserModel : UpdateProfileViewModel
    {
        [Required(ErrorMessage = "Please enter password")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
    }
    public class TransferSuccessViewModel
    {
        public decimal Amount { get; set; }
        public string ReceiverAccount { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
