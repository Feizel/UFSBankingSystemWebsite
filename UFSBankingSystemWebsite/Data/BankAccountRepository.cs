using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystem.Data
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        private readonly AppDbContext _context;

        public BankAccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
