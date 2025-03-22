using FinanceControl.FinanceControl.Domain.Types;
using Quartz;
namespace FinanceControl.FinanceControl.Domain.Entities
{
    public class RecurringTransaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } // Income ou Expense
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CronExpression { get; set; } // Ex: "0 0 5 * *" (todo dia 5 às 00:00)
        public DateTime NextExecution { get; set; } = DateTime.UtcNow; // Padrão: agora
        public DateTime? EndDate { get; set; } // Opcional
        public bool IsActive { get; set; } = true; // Ativo por padrão
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public DateTime? CalculateNextExecution()
        {
            if (string.IsNullOrWhiteSpace(CronExpression))
                return null;

            var cron = new CronExpression(CronExpression);
            var nextExecution = cron.GetNextValidTimeAfter(DateTime.UtcNow);

            return nextExecution?.UtcDateTime;
        }
    }
}
