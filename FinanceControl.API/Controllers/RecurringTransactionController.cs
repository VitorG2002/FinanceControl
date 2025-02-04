using FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecurringTransactionController : ControllerBase
    {
        private readonly IRecurringTransactionService _service;

        public RecurringTransactionController(IRecurringTransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] RecurringTransactionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _service.AddAsync(dto, userId);
            return CreatedAtAction(nameof(GetAllByUser), new { userId }, transaction);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transactions = await _service.GetAllAsync(userId);
            return Ok(transactions);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] RecurringTransactionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _service.UpdateAsync(dto, userId);
            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
