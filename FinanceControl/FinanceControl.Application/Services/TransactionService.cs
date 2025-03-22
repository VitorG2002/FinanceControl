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
        private readonly IRepository<User> _userRepository;

        public TransactionService(ITransactionRepository rep, IRepository<User> userRepository)
        {
            _rep = rep;
            _userRepository = userRepository;
        }


        public async Task<Transaction> AddAsync(TransactionCreateDto dto, string userId)
        {
            var transaction = dto.MapTo<TransactionCreateDto, Transaction>();
            transaction.UserId = int.Parse(userId);
            transaction.CreatedAt = DateTime.UtcNow;

            if (transaction.Type == TransactionType.Expense)
            {
                var user = await _userRepository.GetByIdAsync(int.Parse(userId));
                if (user == null)
                    throw new InvalidOperationException("Usuário não encontrado.");

                var limitType = await CheckAndNotifyExpenseLimitAsync(user, transaction.Amount, transaction.CreatedAt);
                if (!string.IsNullOrEmpty(limitType))
                {
                    var notification = new NotificationMessage
                    {
                        Email = user.Email,
                        Subject = "Limite de Despesa Excedido",
                        Body = $"Você excedeu seu limite de despesa {limitType}."
                    };

                    var publisher = new RabbitMqPublisher();
                    await publisher.PublishNotification(notification);
                }
            }

            return await _rep.AddAsync(transaction);
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Expense)
            {
                var user = await _userRepository.GetByIdAsync(transaction.UserId);
                if (user == null)
                    throw new InvalidOperationException("Usuário não encontrado.");

                var limitType = await CheckAndNotifyExpenseLimitAsync(user, transaction.Amount, transaction.CreatedAt);
                if (!string.IsNullOrEmpty(limitType))
                {
                    var notification = new NotificationMessage
                    {
                        Email = user.Email,
                        Subject = "Limite de Despesa Excedido",
                        Body = $"Você excedeu seu limite de despesa {limitType}."
                    };

                    var publisher = new RabbitMqPublisher();
                    await publisher.PublishNotification(notification);
                }
            }

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

            if (result.Type == TransactionType.Expense)
            {
                var user = await _userRepository.GetByIdAsync(result.UserId);
                if (user == null)
                    throw new InvalidOperationException("Usuário não encontrado.");

                var limitType = await CheckAndNotifyExpenseLimitAsync(user, result.Amount, result.CreatedAt);
                if (!string.IsNullOrEmpty(limitType))
                {
                    var notification = new NotificationMessage
                    {
                        Email = user.Email,
                        Subject = "Limite de Despesa Excedido",
                        Body = $"Você excedeu seu limite de despesa {limitType}."
                    };

                    var publisher = new RabbitMqPublisher();
                    await publisher.PublishNotification(notification);
                }
            }

            await _rep.UpdateAsync(result);

            return result;
        }

        private async Task<string> CheckAndNotifyExpenseLimitAsync(User user, decimal transactionAmount, DateTime transactionDate)
        {
            var expenses = await _rep.GetAllAsync(t => t.UserId == user.Id && t.Type == TransactionType.Expense);

            // Calcula as despesas acumuladas nos períodos relevantes
            var dailyExpenses = expenses.Where(t => t.Date.Date == transactionDate.Date).Sum(t => t.Amount);
            var weeklyExpenses = expenses.Where(t => GetWeekOfYear(t.Date) == GetWeekOfYear(transactionDate) && t.Date.Year == transactionDate.Year).Sum(t => t.Amount);
            var monthlyExpenses = expenses.Where(t => t.Date.Month == transactionDate.Month && t.Date.Year == transactionDate.Year).Sum(t => t.Amount);
            var annualExpenses = expenses.Where(t => t.Date.Year == transactionDate.Year).Sum(t => t.Amount);

            bool limitExceeded = false;
            string limitType = "";

            if (user.DailyLimit.HasValue && (dailyExpenses + transactionAmount) > user.DailyLimit.Value)
            {
                limitExceeded = true;
                limitType = "diário";
            }
            else if (user.WeeklyLimit.HasValue && (weeklyExpenses + transactionAmount) > user.WeeklyLimit.Value)
            {
                limitExceeded = true;
                limitType = "semanal";
            }
            else if (user.MonthlyLimit.HasValue && (monthlyExpenses + transactionAmount) > user.MonthlyLimit.Value)
            {
                limitExceeded = true;
                limitType = "mensal";
            }
            else if (user.AnnualLimit.HasValue && (annualExpenses + transactionAmount) > user.AnnualLimit.Value)
            {
                limitExceeded = true;
                limitType = "anual";
            }

            if (limitExceeded)
            {
                return limitType;
            }

            return null;
        }

        private int GetWeekOfYear(DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
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
