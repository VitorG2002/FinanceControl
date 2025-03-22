using System.ComponentModel.DataAnnotations;

namespace FinanceControl.FinanceControl.Application.DTOs.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O Email é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}
