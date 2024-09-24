using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystem.Data
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
