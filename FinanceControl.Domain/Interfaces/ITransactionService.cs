using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddAsync(TransactionCreateDto Transaction);
        Task<TransactionReadDto> GetByIdAsync(int id);
        Task<List<TransactionReadDto>> GetAllAsync();
        Task<Transaction> UpdateAsync(TransactionUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
