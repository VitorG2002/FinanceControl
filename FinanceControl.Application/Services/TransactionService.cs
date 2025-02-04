using FinanceControl.FinanceControl.Application.Common;
using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Application.Extensions;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using FinanceControl.FinanceControl.Domain.Types;
using NotificationService;

namespace FinanceControl.FinanceControl.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _rep;

        public TransactionService(ITransactionRepository rep)
        {
            _rep = rep;
        }

        public async Task<Transaction> AddAsync(TransactionCreateDto dto, string userId)
        {
            var transaction = dto.MapTo<TransactionCreateDto, Transaction>();
            transaction.UserId = int.Parse(userId); 
            transaction.CreatedAt = DateTime.UtcNow;

            var notification = new NotificationMessage
            {
                Email = "usuario@example.com",
                Subject = "Nova Transação",
                Body = "Uma nova transação foi salva."
            };

            var publisher = new RabbitMqPublisher();
            await publisher.PublishNotification(notification);

            return await _rep.AddAsync(transaction);
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            return await _rep.AddAsync(transaction);
        }

        public async Task<TransactionReadDto> GetByIdAsync(int id, string userId)
        {
            var transaction = await _rep.GetAllAsync(t => t.Id == id && t.UserId == int.Parse(userId));
            var result = transaction.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Transação não encontrada.");

            var dto = result.MapTo<Transaction, TransactionReadDto>();

            return dto;
        }

        public async Task<List<TransactionReadDto>> GetAllAsync(TransactionFilterDto filter, string userId)
        {
            var transactions = await _rep.GetAllAsync(t => t.UserId == int.Parse(userId));

            // Aplicar filtros
            var filteredTransactions = transactions
                .Where(t => (!filter.StartDate.HasValue || t.Date.Date >= filter.StartDate.Value.Date) &&
                            (!filter.EndDate.HasValue || t.Date.Date <= filter.EndDate.Value.Date) &&
                            (!filter.Type.HasValue || t.Type == filter.Type))
                .ToList();

            // Mapear para DTO
            var dtos = filteredTransactions.MapTo<List<Transaction>, List<TransactionReadDto>>();

            return dtos;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var transaction = await _rep.GetAllAsync(t => t.Id == id && t.UserId == int.Parse(userId));
            var result = transaction.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Transação não encontrada.");

            await _rep.DeleteAsync(result.Id);

            return true;
        }

        public async Task<Transaction> UpdateAsync(TransactionUpdateDto dto, string userId)
        {
            var transaction = await _rep.GetAllAsync(t => t.Id == dto.Id && t.UserId == int.Parse(userId));
            var result = transaction.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Transação não encontrada.");

            // Usa o AutoMapper para mapear o DTO para a entidade
            result = dto.MapTo(result);

            await _rep.UpdateAsync(result);

            return result;
        }

        public async Task<MonthlyBalanceDto> GetMonthlyBalanceAsync(int year, int month, string userId)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var transactions = await _rep.GetAllAsync(t => t.UserId == int.Parse(userId));

            var monthlyTransactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            var income = monthlyTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var expense = monthlyTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var balance = income - expense;

            return new MonthlyBalanceDto
            {
                Year = year,
                Month = month,
                Income = income,
                Expense = expense,
                Balance = balance
            };
        }

        public async Task<AnnualBalanceDto> GetAnnualBalanceAsync(int year, string userId)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);

            var transactions = await _rep.GetAllAsync(t => t.UserId == int.Parse(userId));

            var annualTransactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            var income = annualTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var expense = annualTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var balance = income - expense;

            return new AnnualBalanceDto
            {
                Year = year,
                Income = income,
                Expense = expense,
                Balance = balance
            };
        }

        public async Task<List<CategoryBalanceDto>> GetCategoryBalanceAsync(int year, int month, string userId)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var transactions = await _rep.GetAllWithCategoryAsync(t => t.UserId == int.Parse(userId));

            var monthlyTransactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            var categoryBalances = monthlyTransactions
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryBalanceDto
                {
                    CategoryName = g.Key,
                    Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                    Expense = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
                    Balance = g.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount)
                })
                .ToList();

            return categoryBalances;
        }
    }
}
