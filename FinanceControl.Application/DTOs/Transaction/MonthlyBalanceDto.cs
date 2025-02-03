namespace FinanceControl.FinanceControl.Application.DTOs.Transaction
{
    public class MonthlyBalanceDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
    }
}
