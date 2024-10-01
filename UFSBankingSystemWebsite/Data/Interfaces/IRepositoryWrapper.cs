using UFSBankingSystemWebsite.Models;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IRepositoryWrapper
    {
        ITransactionRepository Transactions { get; }
        IReviewRepository Review { get; }
        IBankAccountRepository BankAccount { get; }
        ILoginRepository Logins { get; }
        INotificationRepository Notification { get; }
        IUserRepository AppUser { get; }
        void SaveChanges();
        Task SaveChangesAsync(); // Save Asynchronously
    }
}
