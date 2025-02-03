using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserCreateDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userCreated = await _service.AddAsync(user);

            return CreatedAtAction(nameof(GetAll), new { id = userCreated.Id }, userCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);

            return Ok(user);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var user = await _service.UpdateAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
