using UFSBankingSystemWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystemWebsite.Data
{
    public class FinancialAdvisorRepository : RepositoryBase<FinancialAdvisor>, IFinancialAdvisorRepository
    {
        private readonly AppDbContext _context;

        public FinancialAdvisorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FinancialAdvisor>> GetAllAdvisorsAsync()
        {
            return await _context.FinancialAdvisors.ToListAsync(); // Include user details if needed
        }

        public async Task<FinancialAdvisor> GetAdvisorByIdAsync(int id)
        {
            return await _context.FinancialAdvisors.FirstOrDefaultAsync(f => f.FinancialAdvisorID == id);
        }

        public async Task CreateAdvisorAsync(FinancialAdvisor advisor)
        {
            await _context.FinancialAdvisors.AddAsync(advisor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdvisorAsync(FinancialAdvisor advisor)
        {
            _context.FinancialAdvisors.Update(advisor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdvisorAsync(int id)
        {
            var advisor = await GetAdvisorByIdAsync(id);
            if (advisor != null)
            {
                _context.FinancialAdvisors.Remove(advisor);
                await _context.SaveChangesAsync();
            }
        }
    }
}