namespace UFSBankingSystem.Data.Interfaces
{
    public interface IRepositoryWrapper
    {
        ITransactionRepository Transactions { get; }
        IReviewRepository Review { get; }
        IChargesRepository Charges { get; }
        IAccountRepository BankAccount { get; }
        ILoginRepository Logins { get; }
        INotificationRepository Notification { get; }
        IUserRepository AppUser { get; }
        void SaveChanges();
    }
}
