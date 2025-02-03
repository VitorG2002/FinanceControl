using FinanceControl.FinanceControl.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private static readonly List<Transaction> Transactions = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Transactions);
        }

        [HttpPost]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Invalid transaction data.");

            Transactions.Add(transaction);
            return CreatedAtAction(nameof(GetAll), new { id = transaction.Id }, transaction);
        }
    }
}
