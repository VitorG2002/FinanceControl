using FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction;
using FinanceControl.FinanceControl.Application.Extensions;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;

namespace FinanceControl.FinanceControl.Application.Services
{
    public class RecurringTransactionService : IRecurringTransactionService
    {
        private readonly IRecurringTransactionRepository _rep;
        private readonly ITransactionService _repTransaction;

        public RecurringTransactionService(IRecurringTransactionRepository rep, ITransactionService repTransaction)
        {
            _rep = rep;
            _repTransaction = repTransaction;
        }

        public async Task<RecurringTransaction> AddAsync(RecurringTransactionCreateDto dto, string userId)
        {
            var transaction = dto.MapTo<RecurringTransactionCreateDto, RecurringTransaction>();
            transaction.UserId = int.Parse(userId);
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.NextExecution = (DateTime)transaction.CalculateNextExecution();

            return await _rep.AddAsync(transaction);
        }

        public async Task<List<RecurringTransactionReadDto>> GetAllAsync(string userId)
        {
            var transactions = (await _rep.GetAllAsync(t => t.UserId == int.Parse(userId))).ToList();
            return transactions.MapTo<List<RecurringTransaction>, List<RecurringTransactionReadDto>>();
        }

        public async Task<RecurringTransactionReadDto> GetByIdAsync(int id, string userId)
        {
            var transaction = await _rep.GetAllAsync(t => t.Id == id && t.UserId == int.Parse(userId));
            var result = transaction.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Transação não encontrada.");

            var dto = result.MapTo<RecurringTransaction, RecurringTransactionReadDto>();

            return dto;
        }

        public async Task<RecurringTransaction> UpdateAsync(RecurringTransactionUpdateDto dto, string userId)
        {
            var transaction = await _rep.GetFirstOrDefaultAsync(t => t.Id == dto.Id && t.UserId == int.Parse(userId));

            if (transaction == null)
                throw new InvalidOperationException("Transação recorrente não encontrada.");

            transaction = dto.MapTo(transaction);
            transaction.NextExecution = (DateTime)transaction.CalculateNextExecution();

            await _rep.UpdateAsync(transaction);
            return transaction;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var transaction = await _rep.GetAllAsync(t => t.Id == id && t.UserId == int.Parse(userId));
            var result = transaction.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Transação recorrente não encontrada.");

            await _rep.DeleteAsync(result.Id);
            return true;
        }

        public async Task ProcessRecurringTransactionsAsync()
        {

                var now = DateTime.UtcNow;
                var recurringTransactions = await _rep.GetAllAsync(rt => rt.IsActive && rt.NextExecution <= now);

                foreach (var recurring in recurringTransactions)
                {
                    var transaction = recurring.MapTo<RecurringTransaction, Transaction>();

                    await _repTransaction.AddAsync(transaction);

                    recurring.NextExecution = (DateTime)recurring.CalculateNextExecution();
                    await _rep.UpdateAsync(recurring);
                }

        }

    }
}
