using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {

        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TransactionCreateDto transaction)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionCreated = await _service.AddAsync(transaction);

            return CreatedAtAction(nameof(GetAll), new { id = transactionCreated.Id }, transactionCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]TransactionFilterDto filter)
        {
            var transactions = await _service.GetAllAsync(filter);

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Transaction = await _service.GetByIdAsync(id);

            return Ok(Transaction);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] TransactionUpdateDto dto)
        {
            var transaction = await _service.UpdateAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _service.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("monthly-balance/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyBalance(int year, int month)
        {
            var balance = await _service.GetMonthlyBalanceAsync(year, month);
            return Ok(balance);
        }

        [HttpGet("annual-balance/{year}")]
        public async Task<IActionResult> GetAnnualBalance(int year)
        {
            var balance = await _service.GetAnnualBalanceAsync(year);
            return Ok(balance);
        }

        [HttpGet("category-balance/{year}/{month}")]
        public async Task<IActionResult> GetCategoryBalance(int year, int month)
        {
            var balances = await _service.GetCategoryBalanceAsync(year, month);
            return Ok(balances);
        }
    }
}
