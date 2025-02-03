using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Infraestructure.Persistence;

namespace FinanceControl.FinanceControl.Infraestructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly FinanceControlDbContext _context;

        public UserRepository(FinanceControlDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
