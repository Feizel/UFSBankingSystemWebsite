using UFSBankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data
{
    public class ReviewRepository : RepositoryBase<FeedBack>, IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FeedBack>> GetAllReviewsAsync()
        {
            return await _context.FeedBacks.ToListAsync();
        }

        public async Task<FeedBack> GetReviewByIdAsync(int id)
        {
            return await _context.FeedBacks.FindAsync(id);
        }

        public async Task AddReviewAsync(FeedBack review)
        {
            await _context.FeedBacks.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(FeedBack review)
        {
            _context.FeedBacks.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await GetReviewByIdAsync(id);
            if (review != null)
            {
                _context.FeedBacks.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}