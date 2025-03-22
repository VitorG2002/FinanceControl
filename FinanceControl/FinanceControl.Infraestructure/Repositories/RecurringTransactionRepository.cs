using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.FinanceControl.Infraestructure.Repositories
{
    public class RecurringTransactionRepository : Repository<RecurringTransaction>, IRecurringTransactionRepository
    {
        private readonly FinanceControlDbContext _context;
        public RecurringTransactionRepository(FinanceControlDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecurringTransaction>> GetPendingExecutionsAsync()
        {
            return await _context.RecurringTransactions
                .Where(rt => rt.NextExecution <= DateTime.UtcNow && rt.IsActive)
                .ToListAsync();
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var transactions = await _context.RecurringTransactions.Where(t => t.UserId == userId).ToListAsync();
            _context.RecurringTransactions.RemoveRange(transactions);
            await _context.SaveChangesAsync();
        }
    }
}
