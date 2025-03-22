using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Quartz;

namespace FinanceControl.FinanceControl.Application.Jobs
{
    public class RecurringTransactionJob : IJob
    {
        private readonly IRecurringTransactionService _recurringTransactionService;

        public RecurringTransactionJob(IRecurringTransactionService recurringTransactionService)
        {
            _recurringTransactionService = recurringTransactionService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _recurringTransactionService.ProcessRecurringTransactionsAsync();
        }
    }
}
