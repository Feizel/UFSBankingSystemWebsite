namespace UFSBankingSystem.Models.ViewModels.Admin
{
    public class AdminViewModel
    {
        public string CurrentPage { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<User> Users { get; set; }
        public List<Transaction> ActiveTransactions { get; set; } = new List<Transaction>(); // This can be used to hold active transactions
        public List<Consultant> Consultants { get; set; } = new List<Consultant>();
        public List<FinancialAdvisor> FinancialAdvisors { get; set; } = new List<FinancialAdvisor>();
        public int TotalUsers { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}