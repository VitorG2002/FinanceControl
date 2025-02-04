namespace FinanceControl.FinanceControl.Application.DTOs.User
{
    public class UserLimitsUpdateDto
    {
        public decimal? DailyLimit { get; set; }
        public decimal? WeeklyLimit { get; set; }
        public decimal? MonthlyLimit { get; set; }
        public decimal? AnnualLimit { get; set; }
    }
}
