namespace UFSBankingSystem.Models.ViewModels.Admin
{
    public class IndexPageViewModel
    {
        public string CurrentPage { get; set; }
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public List<Transactions> ActiveTransactions { get; set; } = new List<Transactions>();
        public List<User> Consultants { get; set; } = new List<User>();
        public List<User> FinAdvisor { get; set; } = new List<User>();
        public List<User> TotalUsers { get; set; } = new List<User>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
