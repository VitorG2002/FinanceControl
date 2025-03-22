namespace FinanceControl.FinanceControl.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; } // Adicionado
        public User User { get; set; } // Relacionamento com User
        public List<Transaction> Transactions { get; set; } = new();
        public List<RecurringTransaction> RecurringTransactions { get; set; } = new();
    }
}
