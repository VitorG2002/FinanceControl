using FinanceControl.FinanceControl.Domain.Entities;
using System.Linq.Expressions;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetAllWithCategoryAsync();
        Task<List<Transaction>> GetAllWithCategoryAsync(Expression<Func<Transaction, bool>> predicate);
        Task DeleteByUserIdAsync(int userId);
    }
}
