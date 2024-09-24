using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels.Admin;

namespace UFSBankingSystem.Data.Interfaces
{
    public interface IUserRepository : IRepositoryBase<AppUser>
    {
        Task<List<UserViewModel>> GetAllUsersAndBankAccount();
    }
}
