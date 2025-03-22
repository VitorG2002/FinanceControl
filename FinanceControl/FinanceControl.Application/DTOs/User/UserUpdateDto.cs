namespace FinanceControl.FinanceControl.Application.DTOs.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal? DailyLimit { get; set; }
        public decimal? WeeklyLimit { get; set; }
        public decimal? MonthlyLimit { get; set; }
        public decimal? AnnualLimit { get; set; }
    }
}
