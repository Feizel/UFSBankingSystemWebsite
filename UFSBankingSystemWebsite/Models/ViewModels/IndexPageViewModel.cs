namespace UFSBankingSystem.Models.ViewModels.Admin
{
    public class IndexPageViewModel
    {
        public string CurrentPage { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<Transaction> ActiveTransactions { get; set; } = new List<Transaction>();
        public List<User> Consultants { get; set; } = new List<User>();
        public List<User> FinancialAdvisor { get; set; } = new List<User>();
        public List<User> TotalUsers { get; set; } = new List<User>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
