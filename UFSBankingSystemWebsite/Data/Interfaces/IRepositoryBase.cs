using System.Linq.Expressions;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int? id);
        Task RemoveAsync(int? id);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> FindAllAsync();
        Task<T> FindByIdAsync(int id);
        Task CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);

    }
}
