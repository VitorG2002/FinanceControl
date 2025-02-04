using FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Services
{
    public interface IRecurringTransactionService
    {
        Task<RecurringTransaction> AddAsync(RecurringTransactionCreateDto dto, string userId);
        Task<List<RecurringTransactionReadDto>> GetAllAsync(string userId);
        Task<RecurringTransactionReadDto> GetByIdAsync(int id, string userId);
        Task<RecurringTransaction> UpdateAsync(RecurringTransactionUpdateDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
        Task ProcessRecurringTransactionsAsync();
    }
}
