﻿using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace UFSBankingSystem.Data
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserViewModel>> GetAllUsersAndBankAccount()
        {
            return await (from user in _context.Users
                                  join account in _context.BankAccounts
                                  on user.Email equals account.UserEmail
                                  where user.UserRole == "Student" || user.UserRole == "Staff"
                                  select new UserViewModel
                                  {
                                      AppUser = user,
                                      BankAccount = account,

                                  }).ToListAsync();
        }
    }
}
