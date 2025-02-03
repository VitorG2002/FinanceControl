using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Application.Extensions;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces;

namespace FinanceControl.FinanceControl.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> _rep;

        public TransactionService(IRepository<Transaction> rep)
        {
            _rep = rep;
        }

        public async Task<Transaction> AddAsync(TransactionCreateDto dto)
        {
            Transaction transaction = dto.MapTo<TransactionCreateDto, Transaction>();

            return await _rep.AddAsync(transaction);
        }

        public async Task<TransactionReadDto> GetByIdAsync(int id)
        {
            var transaction = await _rep.GetByIdAsync(id);

            var dto = transaction.MapTo<Transaction, TransactionReadDto>();

            return dto;
        }

        public async Task<List<TransactionReadDto>> GetAllAsync()
        {
            var transactions = (await _rep.GetAllAsync()).ToList();

            var listDtos = transactions.MapTo<List<Transaction>, List<TransactionReadDto>>();

            return listDtos;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _rep.DeleteAsync(id);

            return true;
        }

        public async Task<Transaction> UpdateAsync(TransactionUpdateDto dto)
        {
            var transaction = await _rep.GetByIdAsync(dto.Id);
            transaction = dto.MapTo(transaction);

            await _rep.UpdateAsync(transaction);

            return transaction;
        }
    }
}
