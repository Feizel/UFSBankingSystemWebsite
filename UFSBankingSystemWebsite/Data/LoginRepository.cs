using Microsoft.EntityFrameworkCore;
using UFSBankingSystem.Models;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data
{
    public class LoginRepository : RepositoryBase<LoginSessions>, ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LoginSessions> GetSessionByUserIdAsync(string userId)
        {
            return await _context.LoginSessions.FirstOrDefaultAsync(ls => ls.UserId == userId);
        }

        public async Task<List<LoginSessions>> GetAllSessionsAsync()
        {
            return await _context.LoginSessions.ToListAsync();
        }

        public async Task DeleteSessionAsync(int sessionId)
        {
            var session = await GetByIdAsync(sessionId);
            if (session != null)
            {
                _context.LoginSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }
}