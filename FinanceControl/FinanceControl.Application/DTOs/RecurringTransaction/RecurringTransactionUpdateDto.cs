using FinanceControl.FinanceControl.Domain.Types;

namespace FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction
{
    public class RecurringTransactionUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public int CategoryId { get; set; }
        public string CronExpression { get; set; } // Ex: "0 0 5 * *" (todo dia 5 às 00:00)
        public DateTime? EndDate { get; set; } // Opcional
        public bool IsActive { get; set; } = true; // Ativo por padrão
    }
}
