using UFSBankingSystemWebsite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IFinancialAdvisorRepository : IRepositoryBase<FinancialAdvisor>
    {
        Task<List<FinancialAdvisor>> GetAllAdvisorsAsync(); // Retrieve all financial advisors
        Task<FinancialAdvisor> GetAdvisorByIdAsync(int id); // Retrieve a specific advisor by ID
        Task CreateAdvisorAsync(FinancialAdvisor advisor); // Create a new financial advisor
        Task UpdateAdvisorAsync(FinancialAdvisor advisor); // Update an existing financial advisor
        Task DeleteAdvisorAsync(int id); // Delete a financial advisor by ID
    }
}