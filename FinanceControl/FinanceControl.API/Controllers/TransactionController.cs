using FinanceControl.FinanceControl.Application.DTOs.Transaction;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceControl.FinanceControl.API.Controllers
{

    /// <summary>
    /// Controlador para gerenciamento de transações financeiras
    /// </summary>
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

        /// <summary>
        /// Cria uma nova transação
        /// </summary>
        /// <param name="dto">Dados da transação</param>
        /// <returns>Transação criada</returns>
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

        /// <summary>
        /// Recupera todas as transações do usuário autenticado com base nos filtros fornecidos.
        /// </summary>
        /// <param name="filter">Objeto que contém os critérios de filtro para as transações, como data de início, data de término e tipo de transação.</param>
        /// <returns>Uma lista de transações que correspondem aos critérios de filtro aplicados.</returns>
        /// <response code="200">Transações recuperadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="500">Erro interno do servidor ao processar a solicitação.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TransactionFilterDto filter)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transactions = await _service.GetAllAsync(filter, userId);
            return Ok(transactions);
        }

        /// <summary>
        /// Obtém uma transação pelo ID
        /// </summary>
        /// <param name="id">ID da transação</param>
        /// <returns>Transação encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transaction = await _service.GetByIdAsync(id, userId);
            return Ok(transaction);
        }

        /// <summary>
        /// Atualiza uma transação existente
        /// </summary>
        /// <param name="dto">Dados atualizados da transação</param>
        /// <returns>Nenhum conteúdo</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Exclui uma transação pelo ID
        /// </summary>
        /// <param name="id">ID da transação</param>
        /// <returns>Nenhum conteúdo</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var transaction = await _service.DeleteAsync(id, userId);
            return NoContent();
        }

        /// <summary>
        /// Retorna o saldo mensal do usuário
        /// </summary>
        /// <param name="year">Ano</param>
        /// <param name="month">Mês (1-12)</param>
        /// <returns>Saldo mensal (receitas, despesas e saldo)</returns>
        [HttpGet("monthly-balance/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyBalance(int year, int month)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var balance = await _service.GetMonthlyBalanceAsync(year, month, userId);
            return Ok(balance);
        }

        /// <summary>
        /// Retorna o saldo anual do usuário
        /// </summary>
        /// <param name="year">Ano</param>
        /// <returns>Saldo anual (receitas, despesas e saldo)</returns>
        [HttpGet("annual-balance/{year}")]
        public async Task<IActionResult> GetAnnualBalance(int year)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var balance = await _service.GetAnnualBalanceAsync(year, userId);
            return Ok(balance);
        }

        /// <summary>
        /// Retorna o saldo mensal do usuário por categoria
        /// </summary>
        /// <param name="year">Ano</param>
        /// <param name="month">Mês (1-12)</param>
        /// <returns>Saldo mensal por categoria (receitas, despesas e saldo)</returns>
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
