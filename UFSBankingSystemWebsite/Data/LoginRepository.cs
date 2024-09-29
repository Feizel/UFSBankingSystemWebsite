using Microsoft.EntityFrameworkCore;
using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystemWebsite.Data
{
    public class LoginRepository : RepositoryBase<LoginSession>, ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LoginSession> GetSessionByUserIdAsync(string userId)
        {
            return await _context.LoginSessions.FirstOrDefaultAsync(ls => ls.Id == userId);
        }

        public async Task<List<LoginSession>> GetAllSessionsAsync()
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