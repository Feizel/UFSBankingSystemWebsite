﻿using Microsoft.EntityFrameworkCore;
using UFSBankingSystemWebsite.Data.Interfaces;


namespace UFSBankingSystemWebsite.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _appDbContext;
        private ITransactionRepository _Transaction;
        private IReviewRepository _Review;
        private INotificationRepository _Notification;
        private IBankAccountRepository _bankAccount;
        private IUserRepository _appUser;
        private ILoginRepository _logins;
        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IBankAccountRepository BankAccount
        {
            get
            {
                if (_bankAccount == null)
                {
                    _bankAccount = new BankAccountRepository(_appDbContext);
                }

                return _bankAccount;
            }
        }

        public ILoginRepository Logins
        {
            get
            {
                if (_logins == null)
                {
                    _logins = new LoginRepository(_appDbContext);
                }

                return _logins;
            }
        }

        public ITransactionRepository Transactions
        {
            get
            {
                if (_Transaction == null)
                {
                    _Transaction = new TransactionRepository(_appDbContext);
                }

                return _Transaction;
            }
        }

        public INotificationRepository Notification
        {
            get
            {
                if (_Notification == null)
                {
                    _Notification = new NotificationRepository(_appDbContext);
                }

                return _Notification;
            }
        }

        public IReviewRepository Review
        {
            get
            {
                if (_Review == null)
                {
                    _Review = new ReviewRepository(_appDbContext);
                }

                return _Review;
            }
        }

        public IUserRepository AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    _appUser = new UserRepository(_appDbContext);
                }

                return _appUser;
            }
        }

        public void SaveChanges()
        {
            _appDbContext.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync(); // Call the async version of SaveChanges
        }
    }
}
