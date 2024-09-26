using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<List<UserViewModel>> GetAllUsersAndBankAccountAsync(); // Retrieve all users and their bank accounts
        Task<User> GetUserByIdAsync(string userId); // Get a specific user by ID
        Task UpdateUserAsync(User user); // Update user details
        Task DeleteUserAsync(string userId); // Delete a user by ID
    }
}