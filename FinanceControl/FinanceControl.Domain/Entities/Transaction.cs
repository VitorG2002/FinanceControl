using FinanceControl.FinanceControl.Domain.Types;

namespace FinanceControl.FinanceControl.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
