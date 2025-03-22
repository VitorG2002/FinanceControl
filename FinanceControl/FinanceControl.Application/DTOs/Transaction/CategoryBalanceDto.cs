namespace FinanceControl.FinanceControl.Application.DTOs.Transaction
{
    public class CategoryBalanceDto
    {
        public string CategoryName { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
    }
}
