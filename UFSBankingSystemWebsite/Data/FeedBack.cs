using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystem.Data
{
    public class FeedBackRepository : RepositoryBase<FeedBack>, IFeedBackRepository
    {
        private readonly AppDbContext _context;

        public FeedBackRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FeedBack>> GetAllFeedbackAsync()
        {
            return await _context.FeedBacks.ToListAsync();
        }

        public async Task<FeedBack> GetFeedbackByIdAsync(int id)
        {
            return await _context.FeedBacks.FindAsync(id);
        }

        public async Task CreateFeedbackAsync(FeedBack feedback)
        {
            await _context.FeedBacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedbackAsync(FeedBack feedback)
        {
            _context.FeedBacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            var feedback = await GetFeedbackByIdAsync(id);
            if (feedback != null)
            {
                _context.FeedBacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }
    }
}