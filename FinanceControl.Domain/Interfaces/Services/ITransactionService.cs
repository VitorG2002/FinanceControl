using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<Transaction> AddAsync(TransactionCreateDto Transaction);
        Task<TransactionReadDto> GetByIdAsync(int id);
        Task<List<TransactionReadDto>> GetAllAsync(TransactionFilterDto filter);
        Task<Transaction> UpdateAsync(TransactionUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<MonthlyBalanceDto> GetMonthlyBalanceAsync(int year, int month);
        Task<AnnualBalanceDto> GetAnnualBalanceAsync(int year);
        Task<List<CategoryBalanceDto>> GetCategoryBalanceAsync(int year, int month);
    }
}
