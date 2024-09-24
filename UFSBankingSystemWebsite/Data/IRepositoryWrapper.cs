namespace UFSBankingSystem.Data.Interfaces
{
    public interface IRepositoryWrapper
    {
        ITransactionRepository Transactions { get; }
        

        IReviewRepository Review { get; }
        IChargesRepository Charges { get; }
        IBankAccountRepository BankAccount { get; }
        ILoginRepository Logins { get; }
        INotificationRepository Notification { get; }
        IUserRepository AppUser { get; }
        void SaveChanges();
    }
}
