using UFSBankingSystemWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystemWebsite.Data
{
    public class ConsultantRepository : RepositoryBase<Consultant>, IConsultantRepository
    {
        private readonly AppDbContext _context;

        public ConsultantRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Consultant>> GetAllConsultantsAsync()
        {
            return await _context.Consultants.Include(c => c.User).ToListAsync(); // Include user details if needed
        }

        public async Task<Consultant> GetConsultantByIdAsync(int id)
        {
            return await _context.Consultants.Include(c => c.User).FirstOrDefaultAsync(c => c.ConsultantID == id);
        }

        public async Task CreateConsultantAsync(Consultant consultant)
        {
            await _context.Consultants.AddAsync(consultant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConsultantAsync(Consultant consultant)
        {
            _context.Consultants.Update(consultant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteConsultantAsync(int id)
        {
            var consultant = await GetConsultantByIdAsync(id);
            if (consultant != null)
            {
                _context.Consultants.Remove(consultant);
                await _context.SaveChangesAsync();
            }
        }
    }
}