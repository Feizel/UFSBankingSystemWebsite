using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Models;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IBankAccountRepository : IRepositoryBase<BankAccount>
    {
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync(); // Get all bank accounts
        Task<BankAccount> GetAccountByIdAsync(int id); // Get a specific bank account by ID
        Task CreateAccountAsync(BankAccount account); // Create a new bank account
        Task UpdateAccountAsync(BankAccount account); // Update an existing bank account
        Task DeleteAccountAsync(int id); // Delete a bank account by ID
    }
}