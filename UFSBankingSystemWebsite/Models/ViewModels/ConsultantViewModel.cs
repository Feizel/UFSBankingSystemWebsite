namespace UFSBankingSystem.Models.ViewModels
{
    public class ConsultantViewModel
    {
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
}