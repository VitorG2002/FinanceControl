using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetAllWithCategoryAsync();
    }
}
