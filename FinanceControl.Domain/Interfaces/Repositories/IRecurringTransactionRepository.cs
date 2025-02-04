using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Repositories
{
    public interface IRecurringTransactionRepository : IRepository<RecurringTransaction>
    {
        Task<IEnumerable<RecurringTransaction>> GetPendingExecutionsAsync();
        Task DeleteByUserIdAsync(int userId);
    }
}
