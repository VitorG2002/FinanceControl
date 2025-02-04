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
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RecurringTransaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .Property(t => t.DailyLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .Property(t => t.WeeklyLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .Property(t => t.MonthlyLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .Property(t => t.AnnualLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Relacionamento User -> Category (Sem cascata no banco, mas será tratada no código)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Sem cascata automática

            // Relacionamento User -> Transaction (Sem cascata no banco)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento User -> RecurringTransaction (Sem cascata no banco)
            modelBuilder.Entity<RecurringTransaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.RecurringTransactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Category -> Transaction (Mantendo cascata)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar uma categoria, apaga transações

            // Relacionamento Category -> RecurringTransaction (Mantendo cascata)
            modelBuilder.Entity<RecurringTransaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.RecurringTransactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar uma categoria, apaga transações

            base.OnModelCreating(modelBuilder);
        }


    }
}
