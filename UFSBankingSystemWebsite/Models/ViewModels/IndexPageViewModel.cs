namespace UFSBankingSystem.Models.ViewModels.Admin
{
    public class IndexPageViewModel
    {
        public string CurrentPage { get; set; }
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public List<AppUser> Consultants { get; set; } = new List<AppUser>();
        public List<AppUser> FinAdvisor { get; set; } = new List<AppUser>();
        public List<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
