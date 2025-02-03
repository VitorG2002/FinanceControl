using FinanceControl.FinanceControl.Domain.Types;

namespace FinanceControl.FinanceControl.Application.DTOs.Transaction
{
    public class TransactionFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TransactionType? Type { get; set; }
    }
}
