namespace UFSBankingSystem.Models.ViewModels.Admin
{
    public class UserViewModel
    {
        public User AppUser { get; set; }
        public Account BankAccount { get; set; }
        public string _fullName { get; set; } = string.Empty;
    }
}
