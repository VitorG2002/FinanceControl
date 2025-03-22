using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<Transaction> AddAsync(TransactionCreateDto dto, string userId);
        Task<Transaction> AddAsync(Transaction transaction);
        Task<TransactionReadDto> GetByIdAsync(int id, string userId);
        Task<List<TransactionReadDto>> GetAllAsync(TransactionFilterDto filter, string userId);
        Task<Transaction> UpdateAsync(TransactionUpdateDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
        Task<MonthlyBalanceDto> GetMonthlyBalanceAsync(int year, int month, string userId);
        Task<AnnualBalanceDto> GetAnnualBalanceAsync(int year, string userId);
        Task<List<CategoryBalanceDto>> GetCategoryBalanceAsync(int year, int month, string userId);
    }
}
