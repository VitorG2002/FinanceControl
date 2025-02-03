using FinanceControl.FinanceControl.Domain.Types;

namespace FinanceControl.FinanceControl.Application.DTOs.Transaction
{
    public class TransactionUpdateDto
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
