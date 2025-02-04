using FinanceControl.FinanceControl.Domain.Types;
using System.ComponentModel.DataAnnotations;

namespace FinanceControl.FinanceControl.Application.DTOs.Transaction
{
    public class TransactionUpdateDto
    {
        [Required(ErrorMessage = "O id é obrigatório.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O tipo de transação é obrigatório.")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "A quantia é obrigatória.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int CategoryId { get; set; }

        [StringLength(500, ErrorMessage = "A descrição da categoria deve ter no máximo 500 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A Data é obrigatória.")]
        public DateTime Date { get; set; }
    }
}

