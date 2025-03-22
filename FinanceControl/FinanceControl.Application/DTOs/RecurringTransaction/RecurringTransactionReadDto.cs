using FinanceControl.FinanceControl.Domain.Types;

namespace FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction
{
    public class RecurringTransactionReadDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public int CategoryId { get; set; }
        public string CronExpression { get; set; }
        public DateTime NextExecution { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
