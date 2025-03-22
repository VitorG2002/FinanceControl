using System.ComponentModel.DataAnnotations;

namespace FinanceControl.FinanceControl.Application.DTOs.Category
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "O id é obrigatório.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "A descrição da categoria deve ter no máximo 500 caracteres.")]
        public string Description { get; set; }
    }
}
