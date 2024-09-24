using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

namespace UFSBankingSystem.Data
{
    public class LoginRepository : RepositoryBase<LoginSessions>, ILoginRepository

    {
        public LoginRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}