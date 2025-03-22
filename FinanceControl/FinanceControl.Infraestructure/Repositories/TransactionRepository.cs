using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinanceControl.FinanceControl.Infraestructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly FinanceControlDbContext _context;

        public TransactionRepository(FinanceControlDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllWithCategoryAsync()
        {
            return await _context.Transactions
                .Include(t => t.Category) // Inclui a categoria na consulta
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetAllWithCategoryAsync(Expression<Func<Transaction, bool>> predicate)
        {
            return await _context.Transactions
                .Include(t => t.Category) // Inclui a categoria na consulta
                .Where(predicate)
                .ToListAsync();
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var transactions = await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
            _context.Transactions.RemoveRange(transactions);
            await _context.SaveChangesAsync();
        }
    }
}
