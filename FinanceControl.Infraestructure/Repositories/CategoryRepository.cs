using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.FinanceControl.Infraestructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly FinanceControlDbContext _context;

        public CategoryRepository(FinanceControlDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var categories = await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
            _context.Categories.RemoveRange(categories);
            await _context.SaveChangesAsync();
        }
    }
}
