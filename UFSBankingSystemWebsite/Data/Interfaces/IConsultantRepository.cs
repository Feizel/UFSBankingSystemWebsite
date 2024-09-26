using UFSBankingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IConsultantRepository : IRepositoryBase<Consultant>
    {
        Task<List<Consultant>> GetAllConsultantsAsync(); // Retrieve all consultants
        Task<Consultant> GetConsultantByIdAsync(int id); // Retrieve a specific consultant by ID
        Task CreateConsultantAsync(Consultant consultant); // Create a new consultant
        Task UpdateConsultantAsync(Consultant consultant); // Update an existing consultant
        Task DeleteConsultantAsync(int id); // Delete a consultant by ID
    }
}