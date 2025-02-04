using FinanceControl.FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.FinanceControl.Infraestructure.Persistence
{
    public class FinanceControlDbContext : DbContext
    {
        public FinanceControlDbContext(DbContextOptions<FinanceControlDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            // Configuração de unicidade para o email do usuário
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuração de unicidade para o nome da categoria
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
