using UFSBankingSystem.Models;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface ILoginRepository : IRepositoryBase<LoginSessions>
    {
        Task<LoginSessions> GetSessionByUserIdAsync(string userId); // Retrieve login session by user ID
        Task<List<LoginSessions>> GetAllSessionsAsync(); // Retrieve all login sessions
        Task DeleteSessionAsync(int sessionId); // Delete a specific session by ID
    }
}