using UFSBankingSystem.Models;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IReviewRepository : IRepositoryBase<FeedBack>
    {
        Task<List<FeedBack>> GetAllReviewsAsync(); // Retrieve all feedback
        Task<FeedBack> GetReviewByIdAsync(int id); // Retrieve a specific review by ID
        Task AddReviewAsync(FeedBack review); // Add a new review
        Task UpdateReviewAsync(FeedBack review); // Update an existing review
        Task DeleteReviewAsync(int id); // Delete a review by ID
    }
}