using UFSBankingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data.Interfaces
{
    public interface IFeedBackRepository : IRepositoryBase<FeedBack>
    {
        Task<List<FeedBack>> GetAllFeedbackAsync(); // Retrieve all feedback
        Task<FeedBack> GetFeedbackByIdAsync(int id); // Retrieve a specific feedback by ID
        Task CreateFeedbackAsync(FeedBack feedback); // Create a new feedback
        Task UpdateFeedbackAsync(FeedBack feedback); // Update an existing feedback
        Task DeleteFeedbackAsync(int id); // Delete a feedback by ID
    }
}