using FinanceControl.FinanceControl.Application.DTOs.RecurringTransaction;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceControl.FinanceControl.API.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de transações recorrentes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RecurringTransactionController : ControllerBase
    {
        private readonly IRecurringTransactionService _service;

        public RecurringTransactionController(IRecurringTransactionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria uma nova transação recorrente
        /// </summary>
        /// <param name="dto">Dados da transação recorrente</param>
        /// <returns>Transação recorrente criada</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] RecurringTransactionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _service.AddAsync(dto, userId);
            return CreatedAtAction(nameof(GetAllByUser), new { userId }, transaction);
        }

        /// <summary>
        /// Lista todas as transações recorrentes do usuário
        /// </summary>
        /// <returns>Lista de transações recorrentes</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transactions = await _service.GetAllAsync(userId);
            return Ok(transactions);
        }

        /// <summary>
        /// Atualiza uma transação recorrente existente
        /// </summary>
        /// <param name="dto">Dados atualizados da transação recorrente</param>
        /// <returns>Transação recorrente atualizada</returns>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] RecurringTransactionUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _service.UpdateAsync(dto, userId);
            return Ok(transaction);
        }

        /// <summary>
        /// Exclui uma transação recorrente pelo ID
        /// </summary>
        /// <param name="id">ID da transação recorrente</param>
        /// <returns>Nenhum conteúdo</returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}