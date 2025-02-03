using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
    }
}
