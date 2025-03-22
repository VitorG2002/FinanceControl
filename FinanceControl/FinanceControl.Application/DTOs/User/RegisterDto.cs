using System.ComponentModel.DataAnnotations;

namespace FinanceControl.FinanceControl.Application.DTOs.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}
