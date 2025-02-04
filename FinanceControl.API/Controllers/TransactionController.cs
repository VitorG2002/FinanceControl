using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Add([FromBody] TransactionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transactionCreated = await _service.AddAsync(dto, userId);
            return CreatedAtAction(nameof(GetAll), new { id = transactionCreated.Id }, transactionCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TransactionFilterDto filter)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transactions = await _service.GetAllAsync(filter, userId);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transaction = await _service.GetByIdAsync(id, userId);
            return Ok(transaction);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TransactionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transaction = await _service.UpdateAsync(dto, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transaction = await _service.DeleteAsync(id, userId);
            return NoContent();
        }

        [HttpGet("monthly-balance/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyBalance(int year, int month)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var balance = await _service.GetMonthlyBalanceAsync(year, month, userId);
            return Ok(balance);
        }

        [HttpGet("annual-balance/{year}")]
        public async Task<IActionResult> GetAnnualBalance(int year)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var balance = await _service.GetAnnualBalanceAsync(year, userId);
            return Ok(balance);
        }

        [HttpGet("category-balance/{year}/{month}")]
        public async Task<IActionResult> GetCategoryBalance(int year, int month)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var balances = await _service.GetCategoryBalanceAsync(year, month, userId);
            return Ok(balances);
        }
    }
}
